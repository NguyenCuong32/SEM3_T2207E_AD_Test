$(document).ready(function () {
    $('#timkiem').keyup(function () {
        $('#result').html('');
        var search = $('#timkiem').val();
        if (search != '') {
            var expression = new RegExp(search, "i");
            $.getJSON('/json/products.json', function (data) {
                $.each(data, function (key, value) {
                    if (value.Name.search(expression) != -1) {
                        $('#result').append(
                        '<li style="cursor:pointer; display: flex; max-height: 140px;" class="list-group-item link-class"><img style="object-fit:cover" src="/backend/images/Product/' +
                            value.Thumb +
                        '" width="100" class="" /><div style="padding-left:10px; flex-direction: column; margin-left: 2px; overflow: hidden; text-overflow: ellipsis"><span width="100%" style="font-size:12px;font-weight: 700">' +
                        value.Name +'</span></div></li>');
                    }
                })
            })
        }
    })
    $('#result').on('click', 'li', function () {
        var click_text = $(this).text().split('->');
        $('#timkiem').val($.trim(click_text[0]));
        $("#result").html('');
    });
})
