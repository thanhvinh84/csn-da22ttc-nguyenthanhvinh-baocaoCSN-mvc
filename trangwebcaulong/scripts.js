var productImages = document.querySelectorAll('.card-img-top');

productImages.forEach(function(image) {
    image.addEventListener('mouseenter', function() {
        this.style.transform = 'scale(1.1)';
    });

    image.addEventListener('mouseleave', function() {
        this.style.transform = 'scale(1)';
    });
});

var rolexDeepseaImage = document.getElementById('card-img-top');

card-img-top.addEventListener('click', function() {
    var productPageUrl = 'product_page.html';
    window.location.href = productPageUrl;
});

function addToCart(productName, productPrice) {
    var currentCartCount = parseInt(document.querySelector('.cart-badge').innerText);
    
    document.querySelector('.cart-badge').innerText = currentCartCount + 1;
    
    alert("Đã thêm " + productName + " vào giỏ hàng. Giá: " + productPrice);
    
    return false;
}
// Function to add item to cart
function addToCart(name, price) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    let item = { name: name, price: price, image: '' }; // You may add more properties like image
    cart.push(item);
    localStorage.setItem('cart', JSON.stringify(cart));
    updateCartBadge();
}

// Function to update cart badge
function updateCartBadge() {
    let cartItems = JSON.parse(localStorage.getItem('cart')) || [];
    let cartBadge = document.querySelector('.cart-badge');
    if (cartBadge) {
        cartBadge.textContent = cartItems.length;
    }
}



// Update cart badge on page load
window.onload = updateCartBadge;





