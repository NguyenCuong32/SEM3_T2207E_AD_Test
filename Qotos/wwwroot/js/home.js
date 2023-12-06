var page = 1;
var isLoading = false;
// Scroll to load
function handleScroll() {
  if (window.scrollY + window.innerHeight > $(document).height() - 100 && !isLoading) {
    isLoading = true;
    page++;
    $('.lds-ellipsis').show();
    $.ajax({
      type: 'GET',
      contentType: 'application/json; charset=utf-8',
      url: 'https://localhost:7112/home/page/' + page,
      success: function (response) {
        if (response == 'end') {
          isLoading = false;
          $('.lds-ellipsis').hide();
          $(window).off('scroll', handleScroll);
          return;
        }
        var htmlString = '';
        response.forEach((element) => {
          let isLike = false;
          if (element.likes && element.likes.length > 0) {
            for (let index = 0; index < element.likes.length; index++) {
              if (element.likes[index].userId == userId) {
                isLike = true;
                break;
              }
            }
          }
          htmlString += `<div class="col-4 my-3">
                          <div class="card">
                            <a class="p-3 text-decoration-none link-dark" href="/accounts/&#64;${
                              element.user.userName
                            }/profile">
                              <img style="width: 32px; height: 32px; object-fit: cover" class="rounded-circle" src="${
                                element.user.thumbnail
                              }" alt="">
                              <span class="fw-bold">${element.user.userName}</span>
                            </a>
                            <img class="card-img-top btn-view" src="${
                              element.thumbnail
                            }" alt="Card image" style="cursor: zoom-in;" 
														data-bs-toggle="modal" data-bs-target="#viewPictureModal" data-url="/photos/view/${element.id}">
                            <div class="card-body d-flex">
                              <button class="btn me-2 btn-like ${
                                isLike ? 'btn-outline-danger' : 'btn-outline-secondary'
                              }" data-url="/photos/like/${element.id}">
                                <i class="bi bi-heart-fill"></i>
                              </button>
                              <button class="btn btn-outline-secondary me-auto btn-collection" data-url="${
                                element.id
                              }"><i
								class="bi bi-plus-square"></i></button>
                              <button class="btn btn-outline-secondary btn-download" data-url="/photos/download/${
                                element.id
                              }">
                                <i class="bi bi-download"></i> Download
                              </button>
                            </div>
                          </div>
                        </div>`;
        });
        setTimeout(() => {
          $('#data').append(htmlString);
          $('.lds-ellipsis').hide();
          isLoading = false;
        }, 1000);
      },
      error: function () {
        $(window).off('scroll', handleScroll);
      },
    });
  }
}

// Đăng ký sự kiện scroll với hàm handleScroll
$(window).on('scroll', handleScroll);
