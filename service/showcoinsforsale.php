<?php 
		//set Variables
	$server = $_SERVER['SERVER_NAME'];
	$pk = $_POST['pk'];
	$account = $_POST['account'];
	$pkDir = "../accounts/A071AA88FA3430E9762E59EA7913BA3P/".$account.".txt";
	$pkConfirm = file_get_contents($pkDir);
	
			//get password file
			$password = fgetss('../web.config.', 6);
	
	
	
	//set timezone
		$tz = 'zulu';
		$timestamp = time();
		$dt = new DateTime("now", new DateTimeZone($tz)); //first argument "must" be a string
		$dt->setTimestamp($timestamp); //adjust the object to correct timestamp
	
	
	//current version
	$version = "2.0";
	
	if ($pk == $pkConfirm){
		$dir = "../accounts/".$pk."/Bank/";
		$Ones = glob( $dir . "1.*.ForSale*.stack" );
		$Fives = glob( $dir . "5.*.ForSale*.stack" );
		$TwentyFives = glob( $dir . "25.*.ForSale*.stack" );
		$OneHundreds = glob( $dir . "100.*.ForSale*.stack" );
		$TwoHundredFifties = glob( $dir . "250.*.ForSale*.stack" );
		if ($Ones)  {   $numberOfOnes =   count($Ones);}
		if ($Fives)  {   $numberOfFives =   count($Fives);}
		if ($TwentyFives)  {   $numberOfTwentyFives =   count($TwentFives);}
		if ($OneHundreds)  {   $numberOfOneHundreds =   count($OneHundreds);}
		if ($TwoHundredFifties)  {   $numberOfTwoHundredFifties =   count($TwoHundredFifties);}
		if ($Ones == NULL){$numberOfOnes = 0;}
		if ($Fives == null){$numberOfFives = 0;}
		if ($TwentyFives == NULL){$numberOfTwentyFives = 0;}
		if ($OneHundreds == NULL){$numberOfOneHundreds = 0;}
		if ($TwoHundredFifties == null){$numberOfTwoHundredFifties = 0;}
	
		$response->password = $password;
		$response->bank_server = $server;
		$response->account = $account;
		$response->status = "coins_shown";
		$response->message = "Coins totals returned";
		$response->ones =  $numberOfOnes;
		$response->fives = $numberOfFives;
		$response->twentyfives = $numberOfTwentyFives;		
		$response->hundreds = $numberOfOneHundreds;
		$response->twohundredfifties = $numberOfTwoHundredFifties;
		$response->time = $timeStamp;
		$response->version= $version;
		$responseWrite = json_encode($response);
		
		echo $responseWrite;
		
	}else{
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
