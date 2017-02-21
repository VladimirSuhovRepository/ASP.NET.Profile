$(function(){
$("textarea").keypress( function(e) {
    var chr = String.fromCharCode(e.which);
    var str = "abcdefghIjklnmopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZабвгдеёжзийклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖЗИКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯ.,!?()-+0123456789";
    if (str.indexOf(chr) < 0)
      return false;
});
});
