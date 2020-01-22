

!function ($) {

    $(document).on("click","ul.nav li.parent > a ", function(){          
        $(this).find('em').toggleClass("fa-minus");      
    }); 
    $(".sidebar span.icon").find('em:first').addClass("fa-plus");

    $(".datepicker").mask("99/99/9999");
    $(".mobilePhone").mask("+63 999 999 9999");

    $('.datepicker').datetimepicker({
        format: 'MM/DD/YYYY'
    }).on('dp.show', function (e) {
        var datepicker = $('body').find('.bootstrap-datetimepicker-widget:last'),
            position = datepicker.offset(),
            parent = datepicker.parent(),
            parentPos = parent.offset(),
            width = datepicker.width(),
            parentWid = parent.width();

        // move datepicker to the exact same place it was but attached to body
        datepicker.appendTo('body');
        datepicker.css({
            position: 'absolute',
            top: position.top,
            bottom: 'auto',
            left: position.left,
            right: 'auto'
        });

        // if datepicker is wider than the thing it is attached to then move it so the centers line up
        if (parentPos.left + parentWid < position.left + width) {
            var newLeft = parentPos.left;
            newLeft += parentWid / 2;
            newLeft -= width / 2;
            datepicker.css({ left: newLeft });
        }
    });

    $('ul.timeline li input, ul.timeline li select').focusin(function () {
        $('.timeline-badge').removeClass('primary');
        $(this).closest('li').find('.timeline-badge').addClass('primary');
    });
}

(window.jQuery);
	$(window).on('resize', function () {
  if ($(window).width() > 768) $('#sidebar-collapse').collapse('show')
})
$(window).on('resize', function () {
  if ($(window).width() <= 767) $('#sidebar-collapse').collapse('hide')
})

$(document).on('click', '.panel-heading span.clickable', function(e){
    var $this = $(this);
	if(!$this.hasClass('panel-collapsed')) {
		$this.parents('.panel').find('.panel-body').slideUp();
		$this.addClass('panel-collapsed');
		$this.find('em').removeClass('fa-toggle-up').addClass('fa-toggle-down');
	} else {
		$this.parents('.panel').find('.panel-body').slideDown();
		$this.removeClass('panel-collapsed');
		$this.find('em').removeClass('fa-toggle-down').addClass('fa-toggle-up');
	}
})

jQuery(document).ready(function () {
    $('#myList').DataTable();

    $('.dropdown-toggle').click(function () {
        $(this).parent().removeClass('open');
        $(this).parent().addClass('open');
    });

    //var pathname = window.location.pathname;
    //$('ul.nav li').attr('class', '');
    //$('a[href="' + pathname + '"]').closest("li").attr('class', 'active');
});
