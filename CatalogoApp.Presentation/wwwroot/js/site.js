document.addEventListener('DOMContentLoaded', function () {
    const track = document.getElementById('carouselTrack');
    const viewport = document.getElementById('carouselViewport');
    const prevBtn = document.getElementById('prevBtn');
    const nextBtn = document.getElementById('nextBtn');
    const indicators = document.querySelectorAll('.indicator-dot');
    const cards = document.querySelectorAll('.fifa-card');
    const overlay = document.getElementById('playerModalOverlay');
    const modal = document.getElementById('playerModal');
    const closeBtn = document.getElementById('modalCloseBtn');
    const backBtn = document.getElementById('modalBackBtn');

    if (!track || !viewport || cards.length === 0) return;

    let currentIndex = 0;
    let cardsPerView = getCardsPerView();

    function getCardsPerView() {
        const w = window.innerWidth;
        if (w >= 1200) return 5;
        if (w >= 992) return 4;
        if (w >= 768) return 3;
        if (w >= 576) return 2;
        return 1;
    }

    function getCardWidth() {
        if (cards.length === 0) return 260;
        const card = cards[0];
        const style = window.getComputedStyle(card);
        return card.offsetWidth + parseInt(style.marginRight || 0) + 25;
    }

    function updateCarousel() {
        const cardWidth = getCardWidth();
        const maxIndex = Math.max(0, cards.length - cardsPerView);
        currentIndex = Math.min(currentIndex, maxIndex);
        currentIndex = Math.max(0, currentIndex);

        const offset = currentIndex * cardWidth;
        track.style.transform = 'translateX(-' + offset + 'px)';

        indicators.forEach(function (dot, i) {
            dot.classList.toggle('active', i === currentIndex);
        });

        prevBtn.style.opacity = currentIndex === 0 ? '0.3' : '1';
        nextBtn.style.opacity = currentIndex >= maxIndex ? '0.3' : '1';
    }

    prevBtn.addEventListener('click', function () {
        if (currentIndex > 0) {
            currentIndex--;
            updateCarousel();
        }
    });

    nextBtn.addEventListener('click', function () {
        const maxIndex = Math.max(0, cards.length - cardsPerView);
        if (currentIndex < maxIndex) {
            currentIndex++;
            updateCarousel();
        }
    });

    indicators.forEach(function (dot) {
        dot.addEventListener('click', function () {
            currentIndex = parseInt(this.dataset.index);
            updateCarousel();
        });
    });

    // Touch/Swipe support
    let touchStartX = 0;
    let touchEndX = 0;
    let isDragging = false;

    viewport.addEventListener('touchstart', function (e) {
        touchStartX = e.changedTouches[0].screenX;
        isDragging = true;
    }, { passive: true });

    viewport.addEventListener('touchmove', function (e) {
        if (!isDragging) return;
        touchEndX = e.changedTouches[0].screenX;
    }, { passive: true });

    viewport.addEventListener('touchend', function () {
        if (!isDragging) return;
        isDragging = false;
        const diff = touchStartX - touchEndX;
        const threshold = 50;

        if (Math.abs(diff) > threshold) {
            if (diff > 0) {
                nextBtn.click();
            } else {
                prevBtn.click();
            }
        }
    });

    // Mouse drag support
    let mouseStartX = 0;
    let isMouseDragging = false;

    viewport.addEventListener('mousedown', function (e) {
        mouseStartX = e.clientX;
        isMouseDragging = true;
        viewport.style.cursor = 'grabbing';
        e.preventDefault();
    });

    document.addEventListener('mousemove', function (e) {
        if (!isMouseDragging) return;
    });

    document.addEventListener('mouseup', function (e) {
        if (!isMouseDragging) return;
        isMouseDragging = false;
        viewport.style.cursor = 'grab';
        const diff = mouseStartX - e.clientX;
        if (Math.abs(diff) > 50) {
            if (diff > 0) nextBtn.click();
            else prevBtn.click();
        }
    });

    viewport.style.cursor = 'grab';

    // Keyboard navigation
    document.addEventListener('keydown', function (e) {
        if (overlay.classList.contains('active')) {
            if (e.key === 'Escape') closeModal();
            return;
        }
        if (e.key === 'ArrowLeft') prevBtn.click();
        if (e.key === 'ArrowRight') nextBtn.click();
    });

    // Resize handler
    let resizeTimer;
    window.addEventListener('resize', function () {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function () {
            cardsPerView = getCardsPerView();
            updateCarousel();
        }, 150);
    });

    // ===== MODAL =====
    function openModal(card) {
        var data = card.dataset;

        document.getElementById('modalPhoto').src = data.foto;
        document.getElementById('modalPhoto').alt = data.nombre;
        document.getElementById('modalPhoto').style.display = '';
        document.getElementById('modalFallback').style.display = 'none';
        document.getElementById('modalInitial').textContent = data.nombre.charAt(0);
        document.getElementById('modalFlagLarge').textContent = data.bandera;
        document.getElementById('modalName').textContent = data.nombre;
        document.getElementById('modalFlag').textContent = data.bandera;
        document.getElementById('modalCountry').textContent = data.paiscompleto;
        document.getElementById('modalBirth').textContent = data.nacimiento;
        document.getElementById('modalAltura').textContent = data.altura + ' cm';
        document.getElementById('modalPeso').textContent = data.peso + ' kg';
        document.getElementById('modalPosicion').textContent = data.posicion;
        document.getElementById('modalNumero').textContent = '#' + data.numero;
        document.getElementById('modalPie').textContent = data.pie;
        document.getElementById('modalResumen').textContent = data.resumen;

        // Calculate age
        if (data.nacimientoIso) {
            var birth = new Date(data.nacimientoIso);
            var today = new Date();
            var age = today.getFullYear() - birth.getFullYear();
            var m = today.getMonth() - birth.getMonth();
            if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) age--;
            document.getElementById('modalAge').textContent = age + ' anos';
        }

        // Clubs
        var clubesContainer = document.getElementById('modalClubes');
        clubesContainer.innerHTML = '';
        if (data.clubes) {
            data.clubes.split(',').forEach(function (club) {
                var tag = document.createElement('span');
                tag.className = 'modal-club-tag';
                tag.textContent = club.trim();
                clubesContainer.appendChild(tag);
            });
        }

        // Stats
        document.getElementById('modalGoles').textContent = data.goles;
        document.getElementById('modalAsistencias').textContent = data.asistencias;
        document.getElementById('modalPartidos').textContent = data.partidos;

        // Show modal
        overlay.classList.add('active');
        document.body.style.overflow = 'hidden';
        modal.scrollTop = 0;
    }

    function closeModal() {
        overlay.classList.remove('active');
        document.body.style.overflow = '';
    }

    // Card click handlers
    cards.forEach(function (card) {
        card.addEventListener('click', function (e) {
            if (isMouseDragging) return;
            openModal(this);
        });
    });

    // Close modal handlers
    if (closeBtn) closeBtn.addEventListener('click', closeModal);
    if (backBtn) backBtn.addEventListener('click', closeModal);

    overlay.addEventListener('click', function (e) {
        if (e.target === overlay) closeModal();
    });

    // Initialize
    updateCarousel();
});
