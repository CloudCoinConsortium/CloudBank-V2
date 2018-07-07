<?php 
	$pk = $_POST['pk'];
	$account = $_POST['account'];
	$pkDir = "../accounts/A071AA88FA3430E9762E59EA7913BA3P/"+$account + ".txt";
	$pkConfirm = file_get_contents($pkDir);
	$timeStamp = date("Y-m-d","h:i:sa")
	$version = "2.0"
	
	if ($pk = $pkConfirm){
		
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
		
		$response->account = $account;
		$response->status = "Coins Shown";
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
		
		$response->account = $account;
		$response->status = "Cannot Show Coins";
		$response->message = "Private Key or Account Number incorrect";
		$response->ones =  0;
		$response->fives = 0;
		$response->twentyfives = 0;		
		$response->hundreds = 0;
		$response->twohundredfifties = 0;
		$response->time = $timeStamp;
		$response->version= $version;
		
		$responseWrite = json_encode($response);
		
		echo $responseWrite;
		
	}
?>
