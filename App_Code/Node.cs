
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

    public enum NodeStatus
    {
        Ready,
        NotReady,
    }
    public enum TicketHistory { Untried, Failed, Success };

    public class Node
    {
        /*
         * 
         * This Class Contains the properties of a RAIDA node.
         * 
         * */

        public int NodeNumber;
        public int EchoTime = 0;
        public int MultiDetectTime = 0;
        public String FullUrl;
        public int ReadTimeout;
        public NodeStatus RAIDANodeStatus = NodeStatus.NotReady;
        public bool FailsDetect = false;
        public bool FailsFix = false;
        public bool FailsEcho = false;
        public bool HasTicket = false;
        public TicketHistory ticketHistory = TicketHistory.Untried;
        public MultiDetectResponse MultiResponse = new MultiDetectResponse();
        public String Ticket = "";
        RAIDANode node;
        public NodeEchoResponse echoresult;
      public NodeEchoResponse echresponses;

        //Constructor
        public Node(int NodeNumber)
        {
            this.NodeNumber = NodeNumber;
            FullUrl = GetFullURL();
            Debug.WriteLine(FullUrl);
        }

        public Node(int NodeNumber,RAIDANode node)
        {
            this.NodeNumber = NodeNumber;
            this.node = node;
            FullUrl = GetFullURL();
            FullUrl = "https://" +node.urls[0].url+"/service/";
            Debug.WriteLine(FullUrl);
        }

        public String GetFullURL()
        {
            return "https://RAIDA" + (NodeNumber - 1) + ".cloudcoin.global/service/";
        }

        public void ResetTicket()
        {
            HasTicket = false;
            ticketHistory = TicketHistory.Untried;
            Ticket = "";
        }

        public async Task<string> GetTicketResponse(int nn, int sn, String an, int d)
        {
            RAIDA raida = RAIDA.GetInstance();
            Response get_ticketResponse = new Response();
            get_ticketResponse.fullRequest = FullUrl + "get_ticket?nn=" + nn + "&sn=" + sn + "&an=" + an + "&pan=" + an + "&denomination=" + d;
            DateTime before = DateTime.Now;

            try
            {
            get_ticketResponse.fullResponse = await Utils.GetHtmlFromURL(get_ticketResponse.fullRequest);
                Console.WriteLine(get_ticketResponse.fullResponse);
                DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
                get_ticketResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);

                if (get_ticketResponse.fullResponse.Contains("ticket"))
                {
                    String[] KeyPairs = get_ticketResponse.fullResponse.Split(',');
                    String message = KeyPairs[3];
                    int startTicket = Utils.ordinalIndexOf(message, "\"", 3) + 2;
                    int endTicket = Utils.ordinalIndexOf(message, "\"", 4) - startTicket;
                    get_ticketResponse.outcome = message.Substring(startTicket - 1, endTicket + 1); //This is the ticket or message
                    get_ticketResponse.success = true;
                    HasTicket = true;
                    ticketHistory = TicketHistory.Success;
                    Ticket = get_ticketResponse.outcome;

                }
                else
                {
                    get_ticketResponse.success = false;
                    HasTicket = false;
                    ticketHistory = TicketHistory.Failed;
                }//end if

            }
            catch (Exception ex)
            {
                get_ticketResponse.outcome = "error";
                get_ticketResponse.fullResponse = ex.InnerException.Message;
                get_ticketResponse.success = false;
                HasTicket = false;
                ticketHistory = TicketHistory.Failed;
            }//end try catch
            //return get_ticketResponse;
            return get_ticketResponse.fullResponse;
        }//end get ticket

        public async Task<Response> Echo()
        {
            Response echoResponse = new Response();
            echoResponse.fullRequest = this.FullUrl + "echo";
            DateTime before = DateTime.Now;
            FailsEcho = true;
            //RAIDA_Status.failsEcho[raidaID] = true;
            try
            {
                echoResponse.fullResponse = await Utils.GetHtmlFromURL(echoResponse.fullRequest);
                Debug.WriteLine("Echo From Node - " + NodeNumber + ". " + echoResponse.fullResponse);
                try
                {
                    echoresult = JsonConvert.DeserializeObject<NodeEchoResponse>(echoResponse.fullResponse);
                }
                catch(Exception e)
                {

                }
                //Debug.WriteLine("Echo URL - "+ FullUrl);
                if (echoResponse.fullResponse.Contains("ready"))
                {
                    echoResponse.success = true;
                    echoResponse.outcome = "ready";
                    this.RAIDANodeStatus = NodeStatus.Ready;
                    FailsEcho = false;
                    //RAIDA_Status.failsEcho[raidaID] = false;
                }
                else
                {
                    this.RAIDANodeStatus = NodeStatus.NotReady;
                    echoResponse.success = false;
                    echoResponse.outcome = "error";
                    FailsEcho = true;
                    //RAIDA_Status.failsEcho[raidaID] = true;
                }
            }
            catch (Exception ex)
            {
                echoResponse.outcome = "error";
                echoResponse.success = false;
                this.RAIDANodeStatus = NodeStatus.NotReady;
                FailsEcho = true;
                //RAIDA_Status.failsEcho[raidaID] = true;
                if (ex.InnerException != null)
                    echoResponse.fullResponse = ex.InnerException.Message;
                Debug.WriteLine("Error---" + ex.Message);
            }
            DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
            echoResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);
            EchoTime = Convert.ToInt32(ts.Milliseconds);
            //Debug.WriteLine("Echo Complete-Node No.-" + NodeNumber + ".Status-" + RAIDANodeStatus);
            return echoResponse;
        }//end detect

        public async Task<Response> Detect()
        {
            CloudCoin coin = RAIDA.GetInstance().coin;
            Response detectResponse = new Response();
            detectResponse.fullRequest = this.FullUrl + "detect?nn=" + coin.nn + "&sn=" + coin.sn + "&an=" + coin.an[NodeNumber - 1] + "&pan=" + coin.pan[NodeNumber - 1] + "&denomination=" + coin.denomination + "&b=t";
            DateTime before = DateTime.Now;
            coin.SetAnsToPans();
            try
            {
                detectResponse.fullResponse = await Utils.GetHtmlFromURL(detectResponse.fullRequest);

                DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
                detectResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);
                coin.response[this.NodeNumber - 1] = detectResponse;

                if (detectResponse.fullResponse.Contains("pass"))
                {
                    detectResponse.outcome = "pass";
                    detectResponse.success = true;
                    FailsDetect = false;
                }
                else if (detectResponse.fullResponse.Contains("fail") && detectResponse.fullResponse.Length < 200)//less than 200 incase their is a fail message inside errored page
                {
                    detectResponse.outcome = "fail";
                    detectResponse.success = false;
                    RAIDANodeStatus = NodeStatus.Ready;
                    FailsDetect = true;

                    //RAIDA_Status.failsDetect[RAIDANumber] = true;
                }
                else
                {
                    detectResponse.outcome = "error";
                    detectResponse.success = false;
                    RAIDANodeStatus = NodeStatus.NotReady;
                    FailsDetect = true;
                    //RAIDA_Status.failsDetect[RAIDANumber] = true;
                }

            }
            catch (Exception ex)
            {
                detectResponse.outcome = "error";
                detectResponse.fullResponse = ex.InnerException.Message;
                detectResponse.success = false;
            }
            return detectResponse;
        }//end detect

        /**
         * Method DETECT
         * Sends a Detection request to a RAIDA server
         * @param nn  int that is the coin's Network Number 
         * @param sn  int that is the coin's Serial Number
         * @param an String that is the coin's Authenticity Number (GUID)
         * @param pan String that is the Proposed Authenticity Number to replace the AN.
         * @param d int that is the Denomination of the Coin
         * @return Response object. 
         */
        public async Task<Response> Detect(CloudCoin coin)
        {
            Response detectResponse = new Response();
            detectResponse.fullRequest = this.FullUrl + "detect?nn=" + coin.nn + "&sn=" + coin.sn + "&an=" + coin.an[NodeNumber] + "&pan=" + coin.pan[NodeNumber] + "&denomination=" + coin.denomination + "&b=t";
            DateTime before = DateTime.Now;
            coin.SetAnsToPans();
            try
            {
                detectResponse.fullResponse = await Utils.GetHtmlFromURL(detectResponse.fullRequest);

                DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
                detectResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);
                coin.response[this.NodeNumber] = detectResponse;

                if (detectResponse.fullResponse.Contains("pass"))
                {
                    detectResponse.outcome = "pass";
                    detectResponse.success = true;
                }
                else if (detectResponse.fullResponse.Contains("fail") && detectResponse.fullResponse.Length < 200)//less than 200 incase their is a fail message inside errored page
                {
                    detectResponse.outcome = "fail";
                    detectResponse.success = false;
                    RAIDANodeStatus = NodeStatus.Ready;
                    FailsDetect = true;
                    //RAIDA_Status.failsDetect[RAIDANumber] = true;
                }
                else
                {
                    detectResponse.outcome = "error";
                    detectResponse.success = false;
                    RAIDANodeStatus = NodeStatus.NotReady;
                    FailsDetect = true;
                    //RAIDA_Status.failsDetect[RAIDANumber] = true;
                }

            }
            catch (Exception ex)
            {
                detectResponse.outcome = "error";
                detectResponse.fullResponse = ex.InnerException.Message;
                detectResponse.success = false;
            }
            return detectResponse;
        }//end detect

        public void NewCoin()
        {
            HasTicket = false;
            ticketHistory = TicketHistory.Untried;
            Ticket = "";
            FailsDetect = false;
        }
        public class MultiDetectResponse
        {
            public Response[] responses;
        }
        public async Task<MultiDetectResponse> MultiDetect(int[] nn, int[] sn, String[] an, String[] pan, int[] d, int timeout)
        {
            /*PREPARE REQUEST*/
            Response[] response = new Response[nn.Length];
            for (int i = 0; i < nn.Length; i++)
            {
                response[i] = new Response();
            }

            MultiResponse.responses = new Response[nn.Length];

            //Create List of KeyValuePairs to use as the POST data
            List<KeyValuePair<string, string>> postVariables = new List<KeyValuePair<string, string>>();

            //Loop over String array and add all instances to our bodyPoperties
            for (int i = 0; i < nn.Length; i++)
            {
                postVariables.Add(new KeyValuePair<string, string>("nns[]", nn[i].ToString()));
                postVariables.Add(new KeyValuePair<string, string>("sns[]", sn[i].ToString()));
                postVariables.Add(new KeyValuePair<string, string>("ans[]", an[i]));
                postVariables.Add(new KeyValuePair<string, string>("pans[]", pan[i]));
                postVariables.Add(new KeyValuePair<string, string>("denomination[]", d[i].ToString()));
                //Debug.WriteLine("url is " + this.fullUrl + "detect?nns[]=" + nn[i] + "&sns[]=" + sn[i] + "&ans[]=" + an[i] + "&pans[]=" + pan[i] + "&denomination[]=" + d[i]);

                response[i].fullRequest = this.FullUrl + "detect?nns[]=" + nn[i] + "&sns[]=" + sn[i] + "&ans[]=" + an[i] + "&pans[]=" + pan[i] + "&denomination[]=" + d[i];//Record what was sent
            }

            //convert postVariables to an object of FormUrlEncodedContent
            var dataContent = new FormUrlEncodedContent(postVariables.ToArray());
            DateTime before = DateTime.Now;
            DateTime after;
            TimeSpan ts = new TimeSpan();


            /*MAKE REQEST*/
            string totalResponse = "";
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeout);

            try
            {
                //POST THE REQUEST AND FILL THE ANSER IN totalResponse
                totalResponse = "";
                HttpResponseMessage json;

                using (client)
                {
                    // Console.Write("postHtml await for response: ");
                    json = await client.PostAsync(FullUrl + "multi_detect", dataContent);

                    //Console.Write(".");
                    if (json.IsSuccessStatusCode)//200 status good
                    {
                        totalResponse = await json.Content.ReadAsStringAsync();
                        Debug.WriteLine("RAIDA " + NodeNumber + " returned good: " + json.StatusCode);
                        //  Console.Out.WriteLine(totalResponse);
                    }
                    else //404 not found or 500 error. 
                    {
                        Debug.WriteLine("RAIDA " + NodeNumber + " had an error: " + json.StatusCode);
                        after = DateTime.Now;
                        ts = after.Subtract(before);//Start the timer
                        for (int i = 0; i < nn.Length; i++)
                        {
                            response[i].outcome = "error";
                            response[i].fullResponse = json.StatusCode.ToString();
                            response[i].success = false;
                            response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                            FailsDetect = true;
                            //RAIDA_Status.failsDetect[RAIDANumber] = true;
                        }//end for every CloudCoin note
                        MultiResponse.responses = response;
                        return MultiResponse;//END IF THE REQUEST GOT AN ERROR

                    }//end else 404 or 500

                }//end using

            }
            catch (TaskCanceledException ex)//This means it timed out
            {
                // Console.WriteLine("T1:" + ex.Message);
                after = DateTime.Now;
                ts = after.Subtract(before);//Start the timer
                for (int i = 0; i < nn.Length; i++)
                {
                    response[i].outcome = "noresponse";
                    response[i].fullResponse = ex.Message;
                    response[i].success = false;
                    response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    FailsDetect = true;
                    //RAIDA_Status.failsDetect[RAIDANumber] = true;
                }//end for every CloudCoin note
                MultiResponse.responses = response;

                return MultiResponse;//END IF THE REQUEST FAILED
            }
            catch (Exception ex)//Request failed with some kind of error that did not provide a response. 
            {
                // Console.WriteLine("M1:" + ex.Message);
                after = DateTime.Now;
                ts = after.Subtract(before);//Start the timer
                for (int i = 0; i < nn.Length; i++)
                {
                    response[i].outcome = "error";
                    response[i].fullResponse = ex.Message;
                    response[i].success = false;
                    response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    FailsDetect = true;
                    //RAIDA_Status.failsDetect[RAIDANumber] = true;
                }//end for every CloudCoin note
                MultiResponse.responses = response;
                return MultiResponse;//END IF THE REQUEST FAILED
            }//end catch request attmept


            /* PROCESS REQUEST*/
            after = DateTime.Now;
            ts = after.Subtract(before);//Start the timer
            //Is the request a dud?
            if (totalResponse.Contains("dud"))
            {
                //Mark all Responses as duds
                for (int i = 0; i < nn.Length; i++)
                {
                    response[i].fullResponse = totalResponse;
                    response[i].success = false;
                    response[i].outcome = "dud";
                    response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                }//end for each dud
            }//end if dud
            else
            {
                //Not a dud so break up parts into smaller pieces
                //Remove leading "[{"
                totalResponse = totalResponse.Remove(0, 2);
                //Remove trailing "}]"
                totalResponse = totalResponse.Remove(totalResponse.Length - 2, 2);
                //Split by "},{"
                string[] responseArray = Regex.Split(totalResponse, "},{");
                //Check to see if the responseArray is the same length as the request detectResponse. They should be the same
                if (response.Length != responseArray.Length)
                {
                    //Mark all Responses as duds
                    for (int i = 0; i < nn.Length; i++)
                    {
                        response[i].fullResponse = totalResponse;
                        response[i].success = false;
                        response[i].outcome = "dud";
                        response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    }//end for each dud
                }//end if lenghts are not the same
                else//Lengths are the same so lets go through each one
                {


                    for (int i = 0; i < nn.Length; i++)
                    {
                        if (responseArray[i].Contains("pass"))
                        {
                            response[i].fullResponse = responseArray[i];
                            response[i].outcome = "pass";
                            response[i].success = true;
                            response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                        }
                        else if (responseArray[i].Contains("fail") && responseArray[i].Length < 200)//less than 200 incase there is a fail message inside errored page
                        {
                            response[i].fullResponse = responseArray[i];
                            response[i].outcome = "fail";
                            response[i].success = false;
                            response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                        }
                        else
                        {
                            response[i].fullResponse = responseArray[i];
                            response[i].outcome = "error";
                            response[i].success = false;
                            response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                        }
                    }//End for each response
                }//end if array lengths are the same

            }//End Else not a dud
             //Break the respons into sub responses. 
             //RAIDA_Status.multiDetectTime[NodeNumber] = Convert.ToInt32(ts.Milliseconds);
            MultiResponse.responses = response;
            MultiDetectTime = Convert.ToInt32(ts.Milliseconds);
            return MultiResponse;
        }//End multi detect



        //int[] nn, int[] sn, String[] an, String[] pan, int[] d, int timeout
        public async Task<MultiDetectResponse> MultiDetect()
        {
            
            /*PREPARE REQUEST*/
            try
            {

                var raida =RAIDA.ActiveRAIDA ;
                int[] nn = raida.multiRequest.nn;
                int[] sn = raida.multiRequest.sn;
                String[] an = raida.multiRequest.an[NodeNumber - 1];
                String[] pan = raida.multiRequest.pan[NodeNumber - 1];
                int[] d = raida.multiRequest.d;
                int timeout = raida.multiRequest.timeout;

                Response[] response = new Response[nn.Length];
                for (int i = 0; i < nn.Length; i++)
                {
                    response[i] = new Response();
                }

                //Create List of KeyValuePairs to use as the POST data
                List<KeyValuePair<string, string>> postVariables = new List<KeyValuePair<string, string>>();

                //Loop over String array and add all instances to our bodyPoperties
                for (int i = 0; i < nn.Length; i++)
                {
                    postVariables.Add(new KeyValuePair<string, string>("nns[]", nn[i].ToString()));
                    postVariables.Add(new KeyValuePair<string, string>("sns[]", sn[i].ToString()));
                    postVariables.Add(new KeyValuePair<string, string>("ans[]", an[i]));
                    postVariables.Add(new KeyValuePair<string, string>("pans[]", pan[i]));
                    postVariables.Add(new KeyValuePair<string, string>("denomination[]", d[i].ToString()));
                    // Debug.WriteLine("url is " + this.fullUrl + "detect?nns[]=" + nn[i] + "&sns[]=" + sn[i] + "&ans[]=" + an[i] + "&pans[]=" + pan[i] + "&denomination[]=" + d[i]);

                    response[i].fullRequest = this.FullUrl + "detect?nns[]=" + nn[i] + "&sns[]=" + sn[i] + "&ans[]=" + an[i] + "&pans[]=" + pan[i] + "&denomination[]=" + d[i];//Record what was sent
                }

                //convert postVariables to an object of FormUrlEncodedContent
                var dataContent = new FormUrlEncodedContent(postVariables.ToArray());
                DateTime before = DateTime.Now;
                DateTime after;
                TimeSpan ts = new TimeSpan();


                /*MAKE REQEST*/
                string totalResponse = "";
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(timeout);

                try
                {
                    //POST THE REQUEST AND FILL THE ANSER IN totalResponse
                    totalResponse = "";
                    HttpResponseMessage json;

                    using (client)
                    {
                        // Console.Write("postHtml await for response: ");
                        json = await client.PostAsync(FullUrl + "multi_detect", dataContent);

                        //Console.Write(".");
                        if (json.IsSuccessStatusCode)//200 status good
                        {
                            totalResponse = await json.Content.ReadAsStringAsync();
                            // Console.Out.WriteLine("RAIDA " + NodeNumber + " returned good: " + json.StatusCode);
                            //  Console.Out.WriteLine(totalResponse);
                        }
                        else //404 not found or 500 error. 
                        {
                            Console.Out.WriteLine("RAIDA " + NodeNumber + " had an error: " + json.StatusCode);
                            after = DateTime.Now;
                            ts = after.Subtract(before);//Start the timer
                            for (int i = 0; i < nn.Length; i++)
                            {
                                response[i].outcome = "error";
                                response[i].fullResponse = json.StatusCode.ToString();
                                response[i].success = false;
                                response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                                FailsDetect = true;
                                //RAIDA_Status.failsDetect[RAIDANumber] = true;
                            }//end for every CloudCoin note
                            MultiResponse.responses = response;
                            return MultiResponse;//END IF THE REQUEST GOT AN ERROR

                        }//end else 404 or 500

                    }//end using

                }
                catch (TaskCanceledException ex)//This means it timed out
                {
                    // Console.WriteLine("T1:" + ex.Message);
                    after = DateTime.Now;
                    ts = after.Subtract(before);//Start the timer
                    for (int i = 0; i < nn.Length; i++)
                    {
                        response[i].outcome = "noresponse";
                        response[i].fullResponse = ex.Message;
                        response[i].success = false;
                        response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                        FailsDetect = true;
                        //RAIDA_Status.failsDetect[RAIDANumber] = true;
                    }//end for every CloudCoin note
                    MultiResponse.responses = response;

                    return MultiResponse;//END IF THE REQUEST FAILED
                }
                catch (Exception ex)//Request failed with some kind of error that did not provide a response. 
                {
                    // Console.WriteLine("M1:" + ex.Message);
                    after = DateTime.Now;
                    ts = after.Subtract(before);//Start the timer
                    for (int i = 0; i < nn.Length; i++)
                    {
                        response[i].outcome = "error";
                        response[i].fullResponse = ex.Message;
                        response[i].success = false;
                        response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                        FailsDetect = true;
                        //RAIDA_Status.failsDetect[RAIDANumber] = true;
                    }//end for every CloudCoin note
                    MultiResponse.responses = response;
                    return MultiResponse;//END IF THE REQUEST FAILED
                }//end catch request attmept


                /* PROCESS REQUEST*/
                after = DateTime.Now;
                ts = after.Subtract(before);//Start the timer
                                            //Is the request a dud?

                try
                {
                    DetectResponse[] responses = JsonConvert.DeserializeObject<DetectResponse[]>(totalResponse);

                    for (int i = 0; i < nn.Length; i++)
                    {
                        response[i].fullResponse = totalResponse;
                        if (responses[i].status == "pass")
                            response[i].success = true;
                        else
                            response[i].success = false;

                        response[i].outcome = responses[i].status;
                        response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    }//end for each dud

                }
                catch (Exception e)
                {
                    for (int i = 0; i < nn.Length; i++)
                    {
                        response[i].fullResponse = totalResponse;
                        response[i].success = false;
                        response[i].outcome = "e";
                        response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    }//end for each dud

                }

                //if (totalResponse.Contains("dud"))
                //{
                //    //Mark all Responses as duds
                //    for (int i = 0; i < nn.Length; i++)
                //    {
                //        response[i].fullResponse = totalResponse;
                //        response[i].success = false;

                //        response[i].outcome = "dud";
                //        response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                //    }//end for each dud
                //}//end if dud
                //else
                //{
                //    //Not a dud so break up parts into smaller pieces
                //    //Remove leading "[{"
                //    totalResponse = totalResponse.Remove(0, 2);
                //    //Remove trailing "}]"
                //    totalResponse = totalResponse.Remove(totalResponse.Length - 2, 2);
                //    //Split by "},{"
                //    string[] responseArray = Regex.Split(totalResponse, "},{");
                //    //Check to see if the responseArray is the same length as the request detectResponse. They should be the same
                //    if (response.Length != responseArray.Length)
                //    {
                //        //Mark all Responses as duds
                //        for (int i = 0; i < nn.Length; i++)
                //        {
                //            response[i].fullResponse = totalResponse;
                //            response[i].success = false;
                //            response[i].outcome = "dud";
                //            response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                //        }//end for each dud
                //    }//end if lenghts are not the same
                //    else//Lengths are the same so lets go through each one
                //    {


                //        for (int i = 0; i < nn.Length; i++)
                //        {
                //            if (responseArray[i].Contains("pass"))
                //            {
                //                response[i].fullResponse = responseArray[i];
                //                response[i].outcome = "pass";
                //                response[i].success = true;
                //                response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                //            }
                //            else if (responseArray[i].Contains("fail") && responseArray[i].Length < 200)//less than 200 incase there is a fail message inside errored page
                //            {
                //                response[i].fullResponse = responseArray[i];
                //                response[i].outcome = "fail";
                //                response[i].success = false;
                //                response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                //            }
                //            else
                //            {
                //                response[i].fullResponse = responseArray[i];
                //                response[i].outcome = "error";
                //                response[i].success = false;
                //                response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                //            }
                //        }//End for each response
                //    }//end if array lengths are the same

                //}//End Else not a dud
                 //Break the respons into sub responses. 
                MultiDetectTime = Convert.ToInt32(ts.Milliseconds);
                MultiResponse.responses = response;
                return MultiResponse;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e.Message);

            }
            return null;
        }//End multi detect

        /**
        * Method FIX
        * Repairs a fracked RAIDA
        * @param triad three ints trusted server RAIDA numbers
        * @param m1 string ticket from the first trusted server
        * @param m2 string ticket from the second trusted server
        * @param m3 string ticket from the third trusted server
        * @param pan string proposed authenticity number (to replace the wrong AN the RAIDA has)
        * @return string status sent back from the server: sucess, fail or error. 
        */
        public async Task<Response> Fix(int[] triad, String m1, String m2, String m3, String pan)
        {
            Response fixResponse = new Response();
            DateTime before = DateTime.Now;
            fixResponse.fullRequest = FullUrl + "fix?fromserver1=" + triad[0] + "&message1=" + m1 + "&fromserver2=" + triad[1] + "&message2=" + m2 + "&fromserver3=" + triad[2] + "&message3=" + m3 + "&pan=" + pan;
            DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
            fixResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);

            try
            {
                fixResponse.fullResponse = await Utils.GetHtmlFromURL(fixResponse.fullRequest);
                if (fixResponse.fullResponse.Contains("success"))
                {
                    fixResponse.outcome = "success";
                    fixResponse.success = true;
                }
                else
                {
                    fixResponse.outcome = "fail";
                    fixResponse.success = false;
                }
            }
            catch (Exception ex)
            {//quit
                fixResponse.outcome = "error";
                fixResponse.fullResponse = ex.InnerException.Message;
                fixResponse.success = false;
            }
            return fixResponse;
        }//end fixit


        /**
        * Method GET TICKET
        * Returns an ticket from a trusted server
        * @param nn  int that is the coin's Network Number 
        * @param sn  int that is the coin's Serial Number
        * @param an String that is the coin's Authenticity Number (GUID)
        * @param pan String that is the Proposed Authenticity Number to replace the AN.
        * @param d int that is the Denomination of the Coin
        * @return Response object. 
        */
        public async Task<Response> GetTicket(int nn, int sn, String an, int d)
        {
            RAIDA raida = RAIDA.GetInstance();
            Response get_ticketResponse = new Response();
            get_ticketResponse.fullRequest = FullUrl + "get_ticket?nn=" + nn + "&sn=" + sn + "&an=" + an + "&pan=" + an + "&denomination=" + d;
            DateTime before = DateTime.Now;

            try
            {
                get_ticketResponse.fullResponse = await Utils.GetHtmlFromURL(get_ticketResponse.fullRequest);
                DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
                get_ticketResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);

                if (get_ticketResponse.fullResponse.Contains("ticket"))
                {
                    String[] KeyPairs = get_ticketResponse.fullResponse.Split(',');
                    String message = KeyPairs[3];
                    int startTicket = Utils.ordinalIndexOf(message, "\"", 3) + 2;
                    int endTicket = Utils.ordinalIndexOf(message, "\"", 4) - startTicket;
                    get_ticketResponse.outcome = message.Substring(startTicket - 1, endTicket + 1); //This is the ticket or message
                    get_ticketResponse.success = true;
                    HasTicket = true;
                    ticketHistory = TicketHistory.Success;
                    Ticket = get_ticketResponse.outcome;

                }
                else
                {
                    get_ticketResponse.success = false;
                    HasTicket = false;
                    ticketHistory = TicketHistory.Failed;
                }//end if

            }
            catch (Exception ex)
            {
                get_ticketResponse.outcome = "error";
                get_ticketResponse.fullResponse = ex.InnerException.Message;
                get_ticketResponse.success = false;
                HasTicket = false;
                ticketHistory = TicketHistory.Failed;
            }//end try catch
            return get_ticketResponse;
        }//end get ticket

    }
