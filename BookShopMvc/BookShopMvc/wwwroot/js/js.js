
// Lấy danh sách tất cả các phần tử có class "menu-category"
const categories = document.querySelectorAll('.menu-heading');

// Duyệt qua từng phần tử "menu-category"
categories.forEach(category => {
    // Lắng nghe sự kiện click trên phần tử "menu-category"
    category.addEventListener('click', () => {
        // Tìm phần tử "menu" kế tiếp của "menu-category"
        const menu = category.nextElementSibling;

        // Kiểm tra trạng thái hiện tại của "menu" và thực hiện thao tác hiển thị/ẩn
        if (menu.style.display === 'block') {
            menu.style.display = 'none'; // Ẩn "menu"
        } else {
            menu.style.display = 'block'; // Hiển thị "menu"
        }
    });
});
