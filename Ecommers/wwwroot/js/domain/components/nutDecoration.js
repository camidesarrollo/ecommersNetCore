document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".nut-decoration-container").forEach(container => {
        const count = parseInt(container.dataset.count);
        const size = container.dataset.size;
        const opacity = container.dataset.opacity;
        const nutTypes = container.dataset.nuts.split(",");

        const spacing = 100 / (count + 1);

        for (let i = 0; i < count; i++) {
            const nut = document.createElement("div");
            nut.classList.add("nut-decoration");

            nut.textContent = nutTypes[i % nutTypes.length];

            nut.style.left = `${(i + 1) * spacing}%`;
            nut.style.fontSize = size;
            nut.style.opacity = opacity;
            nut.style.animationDelay = `${i * 2}s`;
            nut.style.animationDuration = `${8 + Math.random() * 4}s`;

            container.appendChild(nut);
        }
    });
});
