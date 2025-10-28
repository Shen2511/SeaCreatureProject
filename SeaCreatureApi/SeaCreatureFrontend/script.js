// --- IMPORTANT! ---
// Change port 7123 to your actual backend port if needed
const API_URL = 'https://localhost:7003/api/SeaCreatures';

const creaturesTableBody = document.getElementById('creatures-table-body');
const addCreatureForm = document.getElementById('add-creature-form');

// --- 1. Function: Load and display all sea creatures ---
async function loadCreatures() {
    try {
        const response = await fetch(API_URL);
        if (!response.ok) throw new Error('Network error');
        const creatures = await response.json();

        creaturesTableBody.innerHTML = ''; // Clear "Loading..."

        if (creatures.length === 0) {
            creaturesTableBody.innerHTML = '<tr><td colspan="5">No creatures found in the database. Add the first one!</td></tr>';
            return;
        }

        creatures.forEach(creature => {
            const tr = document.createElement('tr');

            tr.innerHTML = `
                <td>${creature.name}</td>
                <td>${creature.lifespan} years</td>
                <td>${creature.dietType}</td>
                <td>${creature.habitat}</td>
                <td>
                    <button class="delete-btn" data-id="${creature.id}">X</button>
                </td>
            `;
            creaturesTableBody.appendChild(tr);
        });

    } catch (error) {
        console.error('Error loading creatures:', error);
        creaturesTableBody.innerHTML = '<tr><td colspan="5" style="color: red;">Error loading data. Make sure the backend is running.</td></tr>';
    }
}

// --- 2. Function: Add a new sea creature ---
addCreatureForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const newCreature = {
        name: document.getElementById('creature-name').value,
        lifespan: parseInt(document.getElementById('creature-lifespan').value, 10),
        dietType: document.getElementById('creature-diet').value,
        habitat: document.getElementById('creature-habitat').value
    };

    try {
        const response = await fetch(API_URL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newCreature)
        });

        if (!response.ok) throw new Error('Error adding creature');

        addCreatureForm.reset();
        loadCreatures();

    } catch (error) {
        console.error('Error adding creature:', error);
        alert('Failed to add creature.');
    }
});

// --- 3. Function: Delete a sea creature ---
creaturesTableBody.addEventListener('click', async (e) => {
    if (e.target.classList.contains('delete-btn')) {
        const creatureId = e.target.dataset.id;

        if (!confirm(`Are you sure you want to delete creature with ID: ${creatureId}?`)) {
            return;
        }

        try {
            const response = await fetch(`${API_URL}/${creatureId}`, {
                method: 'DELETE'
            });

            if (!response.ok) throw new Error('Error deleting creature');

            loadCreatures();

        } catch (error) {
            console.error('Error deleting creature:', error);
            alert('Failed to delete creature.');
        }
    }
});

// --- Initial load when page opens ---
document.addEventListener('DOMContentLoaded', loadCreatures);
