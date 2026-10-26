// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function() {
    const particles = document.querySelectorAll('.particle');
    const anims = ['float', 'floatReverse', 'float2','floatReverse2'];
    
    particles.forEach(p => {
        const size = Math.floor(Math.random() * 15) + 10;
        const speed = Math.floor(Math.random() * 20) + 20;
        const delay = (Math.random() * 10).toFixed(1);
        const left = Math.random() * 100;
        const top = Math.random() * 100;
        const anim = anims[Math.floor(Math.random() * anims.length)];

        p.style.width = `${size}px`;
        p.style.height = `${size}px`;
        p.style.left = `${left}%`;
        p.style.top = `${top}%`;
        p.style.animationName = anim;
        p.style.animationDuration = `${speed}s`;
        p.style.animationDelay = `${delay}s`;
    })
})

