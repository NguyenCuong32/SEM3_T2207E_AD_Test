var collectionId;
$('.btn-edit-collection').click(function (e) {
  collectionId = $(this).data('url');
  e.preventDefault();
  $.ajax({
    type: 'GET',
    contentType: 'application/json; charset=utf-8',
    url: '/api/collections/details/' + collectionId,
    success: function (response) {
      $('#collectionNameUpdate').val(response.title);
    },
  });
});

$('#formUpdateCollection').submit(function (e) {
  e.preventDefault();
  let data = {
    title: $('#collectionNameUpdate').val(),
  };
  let type = $(this).attr('method').toLowerCase();
  let url = '';
  if (type == 'post') {
    url = '/collections/update/' + collectionId;
  } else {
    url = '/collections/delete/' + collectionId;
  }
  $.ajax({
    type: type,
    contentType: 'application/json; charset=utf-8',
    url: url,
    data: JSON.stringify(data),
    success: function (response) {
      if (type == 'post') {
        window.location.reload();
      } else {
        window.location.replace(`/accounts/@${userName}/collection`);
      }
    },
  });
});

$('#textDeleteCollection').click(function (e) {
  $(this).hide();
  $('#textUpdateCollection').show();
  e.preventDefault();
  $('.btn-submit-collection').text('Xoá');
  $('.btn-submit-collection').toggleClass('btn-primary btn-danger');
  $('#formUpdateCollection').attr('method', 'delete');
});

$('#textUpdateCollection a').click(function (e) {
  $('#textUpdateCollection').hide();
  $('#textDeleteCollection').show();
  e.preventDefault();
  $('.btn-submit-collection').text('Lưu');
  $('.btn-submit-collection').toggleClass('btn-primary btn-danger');
  $('#formUpdateCollection').attr('method', 'post');
});
