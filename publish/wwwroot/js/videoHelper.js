window.checkVideoPlayback = (videoId, fallbackId) => {
    const video = document.getElementById(videoId);
    const fallback = document.getElementById(fallbackId);

    if (!video || !fallback) return;

    video.muted = true;

    video.play().catch(() => {
        // Impossible de jouer la vidéo automatiquement, affichage du fallback
        video.style.display = 'none';
        fallback.style.display = 'block';
    });

    // Au cas où la vidéo démarre puis s'arrête (ex: iOS)
    video.addEventListener('ended', () => {
        video.style.display = 'none';
        fallback.style.display = 'block';
    });

    video.addEventListener('pause', () => {
        if (!video.ended) {
            video.style.display = 'none';
            fallback.style.display = 'block';
        }
    });
};
