<?php 
//include config file
include ('../config.php');


		//set Variables from post
	$server = $_SERVER['SERVER_NAME'];
	$pk = $_GET['pk'];
	$account = $_GET['account'];
	

	$pkDir = "../accounts/".$pfile."/".$account.".txt";
	$pkConfirm = file_get_contents($pkDir);
	
	
	
	
			//set timezone
		$tz = 'zulu';
		$timestamp = time();
		$dt = new DateTime("now", new DateTimeZone($tz)); //first argument "must" be a string
		$dt->setTimestamp($timestamp); //adjust the object to correct timestamp
	
	
	//current version
	$version = "2.0";
	
	if ($pk === $pkConfirm){
		//find bank folder
		$dir = "../accounts/".$pk."/Bank/";
		
		//count how many coins of each denomination are for sale
		$Ones = glob( $dir . "1.*.ForSale*.stack" );
		$Fives = glob( $dir . "5.*.ForSale*.stack" );
		$TwentyFives = glob( $dir . "25.*.ForSale*.stack" );
		$OneHundreds = glob( $dir . "100.*.ForSale*.stack" );
		$TwoHundredFifties = glob( $dir . "250.*.ForSale*.stack" );
		
		if ($Ones)  {   $numberOfOnes =   count($Ones);}
		if ($Fives)  {   $numberOfFives =   count($Fives);}
		if ($TwentyFives)  {   $numberOfTwentyFives =   count($TwentyFives);}
		if ($OneHundreds)  {   $numberOfOneHundreds =   count($OneHundreds);}
		if ($TwoHundredFifties)  {   $numberOfTwoHundredFifties =   count($TwoHundredFifties);}
	
	//sets nulls to zero	
		if ($Ones == NULL){$numberOfOnes = 0;}
		if ($Fives == null){$numberOfFives = 0;}
		if ($TwentyFives == NULL){$numberOfTwentyFives = 0;}
		if ($OneHundreds == NULL){$numberOfOneHundreds = 0;}
		if ($TwoHundredFifties == null){$numberOfTwoHundredFifties = 0;}
	
	
	//create json file with amount of coins
			$response->bank_server = $server;
			$response->account = $account;
			$response->status = "coins_shown";
			$response->message = "Coins totals returned";
			$response->ones =  $numberOfOnes;
			$response->fives = $numberOfFives;
			$response->twentyfives = $numberOfTwentyFives;		
			$response->hundreds = $numberOfOneHundreds;
			$response->twohundredfifties = $numberOfTwoHundredFifties;
			$response->time = $dt->format('d-m-Y:H:i:s.u');
			$response->version= $version;
			$responseWrite = json_encode($response);
			
			echo $responseWrite;
		
	}else{//returns zeros if account id or private key is incorrect
		$response->bank_server = $server;
		$response->account = $account;
		$response->status = "fail";
		$response->message = "Private Key or Account Number is incorrect";
		$response->ones =  0;
		$response->fives = 0;
		$response->twentyfives = 0;		
		$response->hundreds = 0;
		$response->twohundredfifties = 0;
		$response->time = $dt->format('d-m-Y:H:i:s.u');
		$response->version= $version;
		$responseWrite = json_encode($response);
		
		echo $responseWrite;
		
	}
?>
