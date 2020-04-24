# CloudBank in CloudCoin Wallet

## PHASE 1
Note: Phase 1 will not include a GUI but will be done by configuration file (Unless it is easier to include a GUI to configure)

In phase one, we will quickly get things going. We want to enable four or five remote services that will allow merchants to leverage the CloudCoin Wallet to quickly implement CloudCoin in their projects. 
We will start with one default automatically created user and password that will have access to only one local wallet and one skywallet. 
 
Things to do:
1. Add a new folder to the CloudCoinWallet root folder called "CloudBank"
2. Inside of the CloudBank folder, there will be another folder called "RemoteUsers" and a file called "CloudBank.ini"
3. Inside the RemoteUsers folder, there will be one folder for the default user. The name of the folder will be the will be a composite of the account name and pk (password). 
4. When the CloudBank installs for the first time, it will create an account with a username and password. The user will get to access this in the CloudBank portal. 
5. The  

For now, we will keep things simple. All RemoteUsers will have full control over one local wallet and one skywallet . 
CloudCoin Wallet users will be able to:
1. Turn the CloudBank web service on and off.
2. Configure the port that the CloudBank listents to.
3. Create a new user that has a guid for a password. (Creates a new folder with a name that is the username and password seperated by a period)
4. Use can choose the local and skywallet that will be used by the default account.
5. Can get its external IP address by going to http://bot.whatismyipaddress.com/ (https://www.geeksforgeeks.org/java-program-find-ip-address-computer/)
6  Has a button that allows the server to check and see if it is available publically by trying to talk to itself through its public IP. 
7 . The server can use the same code that we have developed for the java cloudbank that uses SpringBoot

## Important URLs

https://github.com/CloudCoinConsortium/CloudBank-Java

https://github.com/CloudCoinConsortium/CloudBank-Java/blob/master/src/main/java/com/cloudcoin/bank/WebPages.java

Here is the main method: https://github.com/CloudCoinConsortium/CloudBank-Java/blob/master/src/main/java/com/cloudcoin/bank/Main.java

# CloudBank Services to be created in Order of Importance

* Send To Skywallet
* Send To Email via Protonmail. 
* Withdraw one stack
* Deposit one Stack
* Get Receipt
* Receive Payment From Skywallet. 


## SEND TO SKYWALLET
Tells the CloudBank to take money from its default wallet and send it to the account specified. 
https://github.com/CloudCoinConsortium/CloudBank-V2#withdraw-one-stack-service

## WITHDRAW ONE STACK

This return a CloudCOin stack in json form. https://github.com/CloudCoinConsortium/CloudBank-V2#withdraw-one-stack-service
This needs a memo parameter added too. 
Here is a working example: 
https://bank.cloudcoin.global/banking/test_withdraw_one_stack.html

## DEPOSIT ONE STACK
The "deposit one stack" is a fire and forget service. It does not tell the caller if the stack was deposited correctly or not. Instead, it returns a receipt number. The user must call the "Get Receipt" service to see the outcome.
Standard API: https://github.com/CloudCoinConsortium/CloudBank-V2/blob/master/README.md#deposit-service 
Exsiting code line 101 https://github.com/CloudCoinConsortium/CloudBank-Java/blob/master/src/main/java/com/cloudcoin/bank/WebPages.java
There is an working example of this here: https://bank.cloudcoin.global/banking/test_deposit_one_stack.html
Note: The memo parameters has been added recently to the standard and will not be in the old code. 


## GET RECIEPT 
Allows the caller to know if their deposit was good. 

https://github.com/CloudCoinConsortium/CloudBank-V2#withdraw-one-stack-service
Working example: https://bank.cloudcoin.global/banking/test_get_receipt.html

## RECEIVE PAYMENT FROM SKYWALLET 
Tells the CloudBank to download the coins with the memo/tag specified. The CloudBank will need to get the SNs of the 
coins and may get them by calling the "view_reciept" service on the RAIDA. 
https://github.com/CloudCoinConsortium/CloudBank-V2#withdraw-one-stack-service



