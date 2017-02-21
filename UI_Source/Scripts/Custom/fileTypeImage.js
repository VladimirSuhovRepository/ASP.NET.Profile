/** 
 * FileType Constructor:
 * return appointed type of file 
 */ 

function FileType() {
  // Object with file types having own images
  var extension = {
    'txt' : ['txt'],
    'pdf' : ['pdf'],
    'psd' : ['psd'],
    'doc' : ['doc', 'docx'],
    'ppt' : ['ppt', 'pptx'],
    'table' : ['xls', 'xlsx'],
    'image' : ['jpg', 'jpeg', 'png'],
    'archive' : ['rar', 'zip'],
    'web' : ['htm', 'html'],
    'src' : ['hpp', 'h', 'java', 'js', 'cs', 'pl', 'pm', 'php', 'php3', 'phtml', 'py', 'rb', 'rbw']      
  };
  
  //appoint type according extension object;
  this.appointType = function(type) {
    type = type.toLowerCase();
    for (var key in extension) {
      for (var i = 0; i < extension[key].length; i++) { 
       // console.log(extension[key][i]);
        if ( type === extension[key][i] ) {
          type = key;
          return type;
        }
      }
    }
    type = 'other';
    return type;
  }
  
}
  
  var fileType = new FileType();

  function changeImgFileByType(elem) {
    var nameFile = elem.attr('data-file-name');
    var nameArr = nameFile.split('.');
    var extension = nameArr[nameArr.length - 1].substring(0, 3);
    var type = fileType.appointType(extension);
    var Url = elem.find('img').attr('src').split('file-');
    var imgUrl = Url[0] + 'file-' + type + '.png';
    elem.find('img').attr('src', imgUrl);
  }

$(document).ready(function() {
  //change image for File according Type
  $('.prf-artifact[data-file-name]').each(function() {
    changeImgFileByType($(this));
  });
});