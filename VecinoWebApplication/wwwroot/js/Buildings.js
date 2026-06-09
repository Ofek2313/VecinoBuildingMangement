var map = L.map('map').setView([31.5, 34.75], 7);

var leafletIcon = L.icon({
    iconUrl: '/lib/leaflet/images/building-solid-full.svg',
    iconSize: [32, 32],
    iconAnchor: [16, 32],
    popupAnchor: [0, -32]
});

L.tileLayer('https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png').addTo(map);

async function loadMap() {
    const res = await fetch(`http://localhost:5086/Guest/GetBuildingsMap`);
    const buildings = await res.json();

    buildings.forEach(b => {
        if (b.latitude && b.longitude) {
            L.marker([b.latitude, b.longitude], { icon: leafletIcon })
                .addTo(map)
                .bindPopup(`
                        <img src="/Guest/GetBuildingPhoto?buildingId=${b.buildingId}" />
                        <div class="leaflet-popup-footer">
                            <div class="popup-address">${b.address}</div>
                            <span class="popup-badge">Building</span>
                        </div>
                    `);
        }
    });
}
loadMap();