<!--<!DOCTYPE html>-->
<html>
<head>
   <meta charset="utf-8">
    <title>Гурман</title>
<link rel="stylesheet" type="text/css" href="css/style1.css">
</head>
<body>

<form enctype="multipart/form-data" action="upload.php" method="post">
    <input type="hidden" name="MAX_FILE_SIZE" value="100000">
    Send this file: <input name="userfile" type="file">
    <input type="submit" value="Send File">
</form>

</body>
</html>