require('./css/saved-cars.scss');
var jQuery = require('jquery');

(function($) {
    $(function () {
        var dom = $('.member-saved-items');
        var data = dom.data('networkids');
        $.get('/api/saved-cars/list?ids=' + data)
            .then(function (response) {
                console.log(response.Value);
                $.each(response.Value,
                    function (index, item) {
                        var img = dom.find('[data-id="' + item.NetworkId + '"] img');
                        img.attr('src', '//carsales.li.csnstatic.com/carsales/'+ item.Photo +'?aspect=centered&height=85&width=128')
                    });
            })
            .fail(function() {
                alert('failed');
            });
    });
})(jQuery);
