// $('.select-tags').select2({ dropdownParent: $('#uploadModal') });
const editPictureModal = new bootstrap.Modal('#editPictureModal');

$('#editTags').select2({
  dropdownParent: $('#editPictureModal'),
  tags: true,
  placeholder: 'Chọn hoặc thêm tags',
});
// Edit Image
var currentOrigin = window.location.origin;
var currentPictureId;

$('.btn-edit').click(function (e) {
  e.preventDefault();
  let url = currentOrigin + $(this).data('url');
  let tags = [];
  $.ajax({
    type: 'GET',
    contentType: 'application/json; charset=utf-8',
    url: currentOrigin + '/tags/search/?key=all',
    success: function (response) {
      tags = response.map((item) => item.tagName);
    },
  });
  $.ajax({
    type: 'GET',
    contentType: 'application/json; charset=utf-8',
    url: url,
    data: 'data',
    success: function (response) {
      currentPictureId = response.id;
      $('#editDescription').val(response.description);
      $('#editLocation').val(response.location);
      $('#editCamera').val(response.camera);
      let stringHtml = '<option></option>';
      let tagsSelect = response.photoTags.map((item) => item.tag.tagName);
      tags.forEach((element) => {
        if (tagsSelect.includes(element)) {
          stringHtml += `<option value="${element}" selected="selected" >${element}</option>`;
        } else {
          stringHtml += `<option value="${element}">${element}</option>`;
        }
      });
      $('#editThumbnail').attr('src', response.thumbnail);
      $('#editTags').html(stringHtml);
    },
  });
});

$('#formEditPicture').submit(function (e) {
  e.preventDefault();
  var data = {
    description: $('#editDescription').val(),
    location: $('#editLocation').val(),
    camera: $('#editCamera').val(),
    tags: $('#editTags').val(),
  };
  let url = currentOrigin + '/photos/edit/' + currentPictureId;
  $.ajax({
    type: 'POST',
    contentType: 'application/json; charset=utf-8',
    url: url,
    data: JSON.stringify(data),
    success: function (response) {
      editPictureModal.hide();
    },
  });
});

//Delete Image
const deletePhotoModal = new bootstrap.Modal('#deletePhotoModal');
$('.btn-delete').click(function (e) {
  deletePhotoModal.show();
  e.preventDefault();
  let url = currentOrigin + $(this).data('url');
  $('#btnDeletePhoto').click(function (e) {
    e.preventDefault();
    $.ajax({
      type: 'delete',
      contentType: 'application/json; charset=utf-8',
      url: url,
      success: function (response) {
        console.log(response);
        window.location.reload();
      },
    });
  });
});
