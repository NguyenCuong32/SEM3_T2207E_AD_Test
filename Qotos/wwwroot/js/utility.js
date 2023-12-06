export { download, calculateTimeAgo, getItem, setItem };

function download(ImageBase64, dowloadName) {
  var a = document.createElement('a');
  a.href = ImageBase64;
  a.download = dowloadName;
  a.target = '_blank';
  a.click();
}

function calculateTimeAgo(reviewTime) {
  var timeSpan = new Date() - new Date(reviewTime);
  if (timeSpan / 1000 < 60) {
    return 'Vừa xong';
  } else if (timeSpan / (1000 * 60) < 60) {
    var minutes = Math.floor(timeSpan / (1000 * 60));
    return `${minutes} ${minutes === 1 ? 'phút' : 'phút'} trước`;
  } else if (timeSpan / (1000 * 60 * 60) < 24) {
    var hours = Math.floor(timeSpan / (1000 * 60 * 60));
    return `${hours} ${hours === 1 ? 'giờ' : 'giờ'} trước`;
  } else if (timeSpan / (1000 * 60 * 60 * 24) < 30) {
    var days = Math.floor(timeSpan / (1000 * 60 * 60 * 24));
    return `${days} ${days === 1 ? 'ngày' : 'ngày'} trước`;
  } else if (timeSpan / (1000 * 60 * 60 * 24 * 30) < 365) {
    var months = Math.floor(timeSpan / (1000 * 60 * 60 * 24 * 30.44)); // Số ngày trung bình trong một tháng
    return `${months} ${months === 1 ? 'tháng' : 'tháng'} trước`;
  } else {
    var years = Math.floor(timeSpan / (1000 * 60 * 60 * 24 * 365.25)); // Số ngày trung bình trong một năm
    return `${years} ${years === 1 ? 'năm' : 'năm'} trước`;
  }
}

function getItem(item) {
  return JSON.parse(localStorage.getItem(item));
}

function setItem(item, obj) {
  localStorage.setItem(item, JSON.stringify(obj));
}
