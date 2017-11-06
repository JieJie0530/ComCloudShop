$(function(){
//    主菜单
    $(".mainNav li a").click(function(){
        $(this).addClass("hover").parent("li").siblings().find("a").removeClass("hover");
        $(this).find("i").addClass("hover").parents().siblings().find("i").removeClass("hover");
    });
    
//    填写信息的下拉框
    //$(".selectInput").click(function(){
    //    $(this).parent(".editInput").find(".infoSelect").slideToggle();
    //});
    //$(".editInfobox>li .editInput li , .editInfobox>li .editInput li>label").click(function(){
    //    var n = $(this).html();
    //    $(this).parent(".infoSelect").slideUp();
    //    $(this).parents(".editInput").find(".selectInput").html(n);
    //});
    
////   购物车加减
//    $(".add").click(function(){ 
//        var t=$(this).parent().find('input[type="text"]'); 
//        t.val(parseInt(t.val())+1) 
//     }); 
//     $(".reduce").click(function(){ 
//        var t=$(this).parent().find('input[type="text"]'); 
//        t.val(parseInt(t.val())-1) 
//        if(parseInt(t.val())<1){ 
//        t.val(1); 
//        } 
//    }); 
     
    


   
});















