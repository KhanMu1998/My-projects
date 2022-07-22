var cart = {}; // корзина
var data_products;

function init() {

   var hash = window.location.hash.substring(1);
   $.post ("core.php", 
            {"action":"init",
             "type": hash
            },
goodsOut);
}

function setCookie(cname,cvalue,exdays) {
  var d = new Date();
  d.setTime(d.getTime() + (exdays*24*60*60*1000));
  var expires = "expires=" + d.toGMTString();
  document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/Shop";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for(var i = 0; i < ca.length; i++) {
      var c = ca[i];
      while (c.charAt(0) == ' ') {
        c = c.substring(1);
      }
      if (c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
      }
    }
    return "";
  }


 function init_search() {

   var hash = window.location.hash.substring(1);

   $.post ("core.php", {"action":"search", "type": hash}, goodsOut);
} 

function clear_cart() {

   $.post ("core.php", {"action":"clear_cart"}, cart_after_clear);
   
}

function cart_after_clear(data) {
    
    console.log(data);
 }

function call_prod(key_id){
  //console.log(key_id);
  setCookie("gurman_prod_key_id",key_id,30);
  document.getElementById(key_id).style.display='flex';
  
}

function goodsOut(data) {
   
    data = JSON.parse(data);
    data_products = data;

    var out='';
    for (var key in data) {
  
        //out +=`<div class="cart" onclick="document.getElementById('prod${data[key].id}').style.display='flex'">`;   
        out +=`<div class="cart" onclick="call_prod('prod${data[key].id}')">`;                
        out +=`<p class="id_out">${data[key].id}</p>`;
        out +=`<img src="products_img/${data[key].image}" alt="" >`;
        out +=`<p class="name">${data[key].name}<br>`;
        out +=`${data[key].price} ₽/шт </p>`;
        out +=`</div>`;
        
    }
    $('#content_inner').html(out);

    
    var out_prod='';

    if (getCookie("gurman_user_role")== "admin")
    {
        for (var key in data) {
            out_prod +=`<div class="prod_page" id="prod${data[key].id}">`;

            out_prod +=`<form enctype="multipart/form-data" class="modal-content_prod animate" action="cartdelete.php" method="post">`;
            //out_prod +=`<span onclick="document.getElementById(\'id03\').style.display=\'none\'" class="close" title="Close Modal">×</span>`;
       

            //out_prod +=`<form class="modal-content_prod animate" action="cartedit.php" method="POST">`;
            out_prod +=`<span onclick="document.getElementById('prod${data[key].id}').style.display='none'" class="close_prod" >×</span>`;       
            out_prod +=`<img class="prod_image" src="products_img/${data[key].image}" alt="">`;

            out_prod +=`<p><input type="hidden" name="MAX_FILE_SIZE" value="1500000"></p>`;
            out_prod +=`<p><input name="userfile" type="file" id="file" class="inputfile">`;
            //out_prod +=`<label for="file">Выбрать изображение</label></p> `;
            //out_prod +=`<input type="submit" value="Сохранить">`;            
            //out_prod +=`<span onclick="document.getElementById('id03').style.display='flex'" class="close_prod" >?</span>`;

            //out_prod +=`<p class="image_name_prod"> Изображение: <input type="text" value="${data[key].image}" name="prod_img" class="image_admin"></p>`;
            out_prod +=`<p class="name_prod">Наименование товара: <input type="text" value="${data[key].name}" name="prod_name" id="prod_name"></p>`;
            out_prod +=`<p class="name_prod">Тип: <input type="text" value="${data[key].type}" name="prod_type" id="prod_type"> </p>`;
            out_prod +=`<p class="price_prod">Цена: <input type="text" value="${data[key].price}" name="prod_price"> ₽/шт </p>`;
            out_prod +=`<p class="life_prod">Срок годности: <input type="text" value="${data[key].life}" name="prod_life"> месяцев</p>`;
            out_prod +=`<p class="availability_prod"> В наличии: <input type="text" value="${data[key].availability}" name="prod_av"> штук</p>`;
            out_prod +=`<p class="descr_prod">Описание: <br> <textarea wrap="soft" name="prod_desc" id="prod_desc">${data[key].description}</textarea></p>`;
           
            
            out_prod +=`<button class="add-to-cart1" data-id="${key}">Сохранить</button>`;
            
            out_prod +=`<input type="text" value="${data[key].id}" name="prod_id" style ="display: none">`;
            
            
           // out_prod +=`</form>`;
            
           // out_prod +=`<form enctype="multipart/form-data" class="modal-content_prod animate" action="cartdelete.php" method="post">`;
            out_prod +=`<button class="add-to-cart1" data-id="${key}">Удалить</button>`;
            out_prod +=`</form></div>`;
            $('#prod').html(out_prod);
        }
    
    }  else
    {
        for (var key in data) {
            out_prod +=`<div class="prod_page" id="prod${data[key].id}">`;
            out_prod +=`<form class="modal-content_prod animate" action="cart.php" method="POST">`;
            out_prod +=`<span onclick="document.getElementById('prod${data[key].id}').style.display='none'" class="close_prod" >×</span>`;       
            out_prod +=`<img class="prod_image" src="products_img/${data[key].image}" alt="">`;       
            out_prod +=`<p class="name_prod">Наименование товара: ${data[key].name}</p>`;
            out_prod +=`<div class="price_prod">Цена: ${data[key].price} ₽/шт </p></div>`;
            out_prod +=`<p class="life_prod">Срок годности: ${data[key].life} месяцев</p>`;
            out_prod +=`<p class="availability_prod"> В наличии: ${data[key].availability} штук</p>`;
            out_prod +=`<p class="descr_prod"> ${data[key].description}</p>`;
            out_prod +=`<label class="amount_label" for="amount">Укажите количество: </label>`;
            out_prod +=`<input type="text" placeholder="" value="1" name="amount">`;
            out_prod +=`<button class="add-to-cart1" data-id="${key}">В корзину</button>`;
            out_prod +=`<input type="text" value="${data[key].id}" name="prod_id" style ="display: none">`;
            
            out_prod +=`</form></div>`;
            $('#prod').html(out_prod);
        }

    }
    
    
    
    //$('.add-to-cart').on('click', addToCart);
}

function cartOut(data) {
    // вывод на страницу

    data = JSON.parse(data);
    
    //console.log(data);
    
    var ind = 0;
    var out_prod='';
    out_prod +=`<div class="login_container" id="cartprod">`;
    out_prod +=`<form class="modal-content_cart animate" >`;
    out_prod +=`<div><span onclick="document.getElementById('cartprod').style.display='none'" class="close_cart" >×</span></div>`;         
    for (var key in data) {
        ind = ind +1;
          
        //out_prod +=`<img class="prod_image" src="products_img/${data[key].image}" alt="">`;       
        out_prod +=`<div class="name_prod"> <img src="products_img/${data[key].image}">`;
        out_prod +=` №: ${ind},`;
        out_prod +=` Наименование : ${data[key].name},`;
        out_prod +=` Цена: ${data[key].price} ₽/шт,`;
        out_prod +=` Количество : ${data[key].c_count}`;
        out_prod +=` </div>`;

    }
    out_prod +=` <div><button class="add-to-cart1" data-id="${key}" onclick="clear_cart()" >Очистить корзину</button>`;
    out_prod +=` <button class="add-to-cart1" data-id="${key}">Оформить заказ</button></div>`;
    out_prod +=`</form></div>`;
    $('#cartcontainer').html(out_prod);
    
}

function init_cart() {
   $.post ("core.php", {"action":"init_cart"}, cartOut);
} 

$(document).ready(function () {
    init();
  
});




var a = window.location.hash.substring(1);
                $(function(){
                 if ((a == "icecream") || (a == "")) {
                    $('#side tr:nth-child(1)').css('background', '#bef7ff')
                 } 
                 else if (a == "bread"){
                    $('#side tr:nth-child(2)').css('background', '#bef7ff')
                 } 
                 else if (a == "milk"){
                    $('#side tr:nth-child(3)').css('background', '#bef7ff')
                 } 
                 else if (a == "fish"){
                    $('#side tr:nth-child(4)').css('background', '#bef7ff')
                 } 
                 else if (a == "fruits"){
                    $('#side tr:nth-child(5)').css('background', '#bef7ff')
                 }  
                 else if (a == "Coffee"){
                    $('#side tr:nth-child(6)').css('background', '#bef7ff')
                 } 
                 else if (a == "nuts"){
                    $('#side tr:nth-child(7)').css('background', '#bef7ff')
                 } 
                });