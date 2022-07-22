<?php


// Соединямся с БД
$link=mysqli_connect("localhost", "root", "", "stu8");
$prod_id = mysqli_real_escape_string($link,$_POST['prod_id']);
$amount = mysqli_real_escape_string($link,$_POST['amount']);
$user   = $_COOKIE["gurman_user_id"];


   
    $query = mysqli_query($link,"SELECT c_id, c_count FROM cart WHERE u_id='".$user."' and p_id='".$prod_id."' LIMIT 1");
    

    if (mysqli_num_rows($query) > 0){  
        $data= mysqli_fetch_assoc( $query );
        $sql = "UPDATE cart SET c_count=c_count+".$amount." WHERE c_id=".$data['c_id'];

        if (mysqli_query($link, $sql)) {
        //echo "Record updated successfully";
        header("Location: main_page.php"); 
        } else {
        echo "Error updating record: " . mysqli_error($link);
        }

    }
    else
    {   $sql = "INSERT INTO cart (u_id, p_id, c_count)
        VALUES (".$user.", ".$prod_id.", ".$amount.")";
        
        if (mysqli_query($link, $sql)) {
          //echo "New record created successfully";
          header("Location: main_page.php"); 
        } else {
          echo "Error: " . $sql . "<br>" . mysqli_error($link);
        }
        


    }

    mysqli_close($link);


?>