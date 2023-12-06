import { download, calculateTimeAgo, getItem, setItem } from './utility.js';

$(document).ready(function () {
  // $('.select-tags').select2({ dropdownParent: $('#uploadModal') });
  const uploadModal = new bootstrap.Modal('#uploadModal');

  $('#tags').select2({
    dropdownParent: $('#uploadModal'),
    tags: true,
    placeholder: 'Chọn hoặc thêm tags',
  });

  let userId = '@userId';

  $('#FileImg').change(function () {
    const file = this.files[0];
    if (file) {
      let reader = new FileReader();
      reader.onload = function (event) {
        $('#FileImgPreview').attr('src', event.target.result);
      };
      reader.readAsDataURL(file);
    }
    $('#upload-first-step').hide();
    $('#upload-second-step').show();
    $('#btnSubmitUploadImg').prop('disabled', false);
    $('#btnSubmitUploadImg').parent().css('cursor', 'pointer');
    $('#Location').focus();
  });
});

var currentOrigin = window.location.origin;

$(document).on('click', '.btn-like', function (e) {
  e.preventDefault();
  let url = `${currentOrigin}${$(this).data('url')}`;
  $(this).toggleClass('btn-outline-danger');
  $(this).toggleClass('btn-outline-secondary');
  $.ajax({
    type: 'get',
    contentType: 'application/json; charset=utf-8',
    url: url,
    success: function (response) {
      console.log(response);
    },
    error: function () {
      window.location.replace('/accounts/login');
    },
  });
});

// Dowload Picture
$(document).on('click', '.btn-download', function (e) {
  e.preventDefault();
  let url = currentOrigin + $(this).data('url');
  console.log(url);
  $.ajax({
    type: 'get',
    contentType: 'application/json; charset=utf-8',
    url: url,
    success: function (response) {
      let dowloadName = `${response.user.userName} - ${Date.now()} - Qotos.${response.extension}`;
      let imgElement = response.thumbnail;
      download(imgElement, dowloadName);
    },
    error: function () {
      console.log('Error');
    },
  });
});

// View Image
$(document).on('click', '.btn-view', function (e) {
  e.preventDefault();
  let url = `${currentOrigin}${$(this).data('url')}`;
  $.ajax({
    type: 'get',
    contentType: 'application/json; charset=utf-8',
    url: url,
    success: function (response) {
      //Tag
      var stringHtml = '';
      response.photoTags.forEach((element) => {
        stringHtml += `<a  class="btn btn-secondary me-2 btn-tag" href="/photos/tag/${element.tag.tagName}">${element.tag.tagName}</a>`;
      });
      $('#photoTags').html(stringHtml);
      //
      $('#authorProfile').attr('href', `/accounts/@${response.user.userName}/profile`);
      $('#authorThumbnail').attr('src', response.user.thumbnail);
      $('#authorUserName').text(response.user.userName);

      // Handle Download
      $('#photoDownload').attr('data-url', `/photos/download/${response.id}`);
      $('#photoCollection').attr('data-url', `${response.id}`);
      // Handle Change Image
      $('#photoThumbnail').attr('src', `${response.thumbnail}`);
      //Check like
      $('#photoLike').attr('data-url', `/photos/like/${response.id}`);

      //Report
      $('#photoReport').attr('data-url', `/photos/report/${response.id}`);

      let isLike = false;
      if (response.likes && response.likes.length > 0 && userId) {
        for (let index = 0; index < response.likes.length; index++) {
          if (response.likes[index].userId == userId) {
            isLike = true;
            break;
          }
        }
      }
      if (isLike) {
        $('#photoLike').addClass('btn-outline-danger');
        $('#photoLike').removeClass('btn-outline-secondary');
      } else {
        $('#photoLike').removeClass('btn-outline-danger');
        $('#photoLike').addClass('btn-outline-secondary');
      }
      //Publish
      $('#photoPublish').text(calculateTimeAgo(response.publish));
      //Like count
      $('#photoLikeCount').text(response.likes.length);
      //View count
      $('#photoViewCount').text(response.views);
      //Download count
      $('#photoDownloadCount').text(response.downloads);
    },
    error: function () {
      console.log('Error');
    },
  });
});

//Search
$('#formSearch').submit(function (e) {
  e.preventDefault();
  let key = $('#searchKey').val();
  if (key.trim() == '') {
    return;
  }
  let tags = getItem('tags');
  if (tags == null) {
    setItem('tags', []);
  }
  let tagExists = false;
  tags.forEach((tag) => {
    if (tag == key) {
      tagExists = true;
    }
  });
  if (tagExists == false) {
    tags.unshift(key);
    setItem('tags', tags);
  }
  location.replace('/photos/tag/' + key);
});

//
let searchBox = $('#searchBox');
let searchStart = $('#searchBox #dataStart');
let searchTagResults = $('#searchBox #dataTags');

$('#searchKey').focus(function (e) {
  showRecentSearch();
  searchBox.show();
  searchStart.show();
  searchTagResults.hide();
  $.ajax({
    type: 'get',
    url: '/tags/search/?key=hot',
    success: function (response) {
      let htmlString = '';
      if (response.length == 0) {
        htmlString = 'Thử với từ khoá khác';
      } else {
        response.forEach((element) => {
          htmlString += `<a class="btn btn-outline-secondary me-1 btn-tag" href="/photos/tag/${element.tagName}">${element.tagName}</a>`;
        });
      }
      $('#searchHot').html(htmlString);
    },
  });
});

$('#searchKey').keyup(function (e) {
  searchBox.show();
  let key = $(this).val();
  if (key.trim() == '') {
    searchStart.show();
    searchTagResults.hide();
  } else {
    searchStart.hide();
    searchTagResults.show();
    $.ajax({
      type: 'get',
      url: '/tags/search/' + key,
      success: function (response) {
        let htmlString = '';
        if (response.length == 0) {
          htmlString = 'Thử với từ khoá khác';
        } else {
          response.forEach((element) => {
            htmlString += `<div class="w-100">
															<a href="/photos/tag/${element.tagName}" class="w-100 btn btn-light text-start btn-tag">${element.tagName}</a>
														</div>`;
          });
        }

        $('#dataTags').html(htmlString);
      },
    });
  }
});

$('#searchKey').blur(function (e) {
  setTimeout(() => {
    searchBox.hide();
  }, 1000);
});

function showRecentSearch() {
  $('#searchRecentBox').show();
  let tags = getItem('tags');
  if (tags == null || tags.length == 0) {
    $('#searchRecentBox').hide();
    return;
  }
  let htmlString = '';
  tags.forEach((element) => {
    htmlString += `<a class="btn btn-outline-secondary me-1 btn-tag" href="/photos/tag/${element}">${element}</a>`;
  });
  $('#searchRecent').html(htmlString);
}

$('#btnDeleteTagsRecent').click(function (e) {
  e.preventDefault();
  $('#searchRecentBox').hide();
  localStorage.removeItem('tags');
});

//

var photoId;
const collectionModal = new bootstrap.Modal('#collectionModal');

$(document).on('click', '.btn-collection', function (e) {
  e.preventDefault();
  photoId = $(this).data('url');
  $.ajax({
    type: 'GET',
    contentType: 'application/json; charset=utf-8',
    url: '/collections/index',
    success: function (response) {
      collectionModal.show();
      let stringHtml = '';
      response.forEach((element) => {
        let photoIds = element.photoCollections.map((item) => {
          return item.photoId;
        });
        if (photoIds.includes(photoId)) {
          stringHtml += `<div class="border rounded p-2 mb-2 d-flex bg-opacity-25 bg-success btn-add-collectionphoto" 
					style="cursor : pointer" data-url="${element.id}">
						<div class="me-auto">
							<small class="number-photos-in-collection">${element.photoCollections.length ?? 0}</small><small> Ảnh</small>
							<div class="fw-bold">${element.title}</div>
						</div>
						<div class="text-success photo-in-collection-status">
							<i class="bi bi-dash-lg"></i>
						</div>
					</div>`;
        } else {
          stringHtml += `<div class="border rounded p-2 mb-2 d-flex bg-opacity-25 bg-light btn-add-collectionphoto" 
					style="cursor : pointer" data-url="${element.id}">
						<div class="me-auto">
							<small class="number-photos-in-collection">${element.photoCollections.length ?? 0}</small> <small> Ảnh</small>
							<div class="fw-bold">${element.title}</div>
						</div>
						<div class="text-success photo-in-collection-status">
						<i class="bi bi-plus-lg"></i>
						</div>
					</div>`;
        }
      });
      $('#collectionsResult').html(stringHtml);
    },
    error: function () {
      window.location.replace('/accounts/login');
    },
  });
});

$('#formAddCollection').submit(function (e) {
  e.preventDefault();
  let title = $('#collectionName').val();
  if (title.trim() == '') {
    return;
  }
  let data = {
    title: title,
  };
  console.log(data);
  $.ajax({
    type: 'POST',
    contentType: 'application/json; charset=utf-8',
    url: '/collections/create/' + photoId,
    data: JSON.stringify(data),
    success: function (response) {
      if (response == 'Exists') {
        $('#collectionName').val('');
        $('#collectionName').blur();
        return;
      }
      console.log(response);
      let content = `<div class="border rounded p-2 mb-2 d-flex bg-opacity-25 bg-success btn-add-collectionphoto" 
												style="cursor : pointer" data-url="${response}">
													<div class="me-auto">
														<small class="number-photos-in-collection">1 </small> <small>Ảnh</small>
														<div class="fw-bold">${title}</div>
													</div>
													<div class="text-success photo-in-collection-status">
														<i class="bi bi-check-lg"></i>
													</div>
												</div>`;
      $('#collectionsResult').prepend(content);
      $('#collectionName').val('');
      $('#collectionName').blur();
    },
    error: function (e) {
      console.log(e);
    },
  });
});

// Add collection photo
$('#collectionModal').on('click', '.btn-add-collectionphoto', function (e) {
  e.preventDefault();
  let that = $(this);
  let numberPhotosInCollection = $(this).find('.number-photos-in-collection').text();
  console.log('So phan tu la :' + numberPhotosInCollection);
  $(this).toggleClass('bg-light bg-success');
  let data = $(this).data('url');
  let url = '/collections/add/' + data + '?photoId=' + photoId;
  $.ajax({
    type: 'GET',
    contentType: 'application/json; charset=utf-8',
    url: url,
    success: function (response) {
      if (response == 'increase') {
        numberPhotosInCollection++;
        that.find('.photo-in-collection-status').html('<i class="bi bi-dash-lg"></i>');
      } else if (response == 'decrease') {
        numberPhotosInCollection--;
        that.find('.photo-in-collection-status').html('<i class="bi bi-plus-lg"></i>');
      }
      that.find('.number-photos-in-collection').text(numberPhotosInCollection);
      that.find('.number-photos-in-collection').text(numberPhotosInCollection);
    },
  });
});

// Report
$('.btn-report').click(function (e) {
  $(this).toggleClass('btn-outline-secondary btn-secondary');
  e.preventDefault();
  let url = $(this).data('url');
  console.log(url);
  $.ajax({
    type: 'GET',
    contentType: 'application/json; charset=utf-8',
    url: url,
    success: function (response) {
      console.log(response);
    },
  });
});

//Test jqueryvalidation
