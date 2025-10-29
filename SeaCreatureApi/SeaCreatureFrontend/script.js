// --- ФІНАЛЬНЕ ПОСИЛАННЯ НА API (Вписано напряму) ---
const API_URL = 'https://seacreatureproject-fuka.up.railway.app/api/SeaCreatures';

const creaturesTableBody = document.getElementById('creatures-table-body');
const addCreatureForm = document.getElementById('add-creature-form');

// --- 1. Функція: Завантажити і показати всіх мешканців ---
async function loadCreatures() {
    try {
        const response = await fetch(API_URL);
        if (!response.ok) throw new Error('Network error');
        const creatures = await response.json();

        creaturesTableBody.innerHTML = ''; // Очищуємо "Loading..."

        if (creatures.length === 0) {
            creaturesTableBody.innerHTML = '<tr><td colspan="5">No creatures found in the database. Add the first one!</td></tr>';
            return;
        }

        creatures.forEach(creature => {
            const tr = document.createElement('tr');

            const tdName = document.createElement('td');
            tdName.textContent = creature.name;
            tr.appendChild(tdName);

            const tdLifespan = document.createElement('td');
            tdLifespan.textContent = ${ creature.lifespan } years;
            tr.appendChild(tdLifespan);

            const tdDiet = document.createElement('td');
            tdDiet.textContent = creature.dietType;
            tr.appendChild(tdDiet);

            const tdHabitat = document.createElement('td');
            tdHabitat.textContent = creature.habitat;
            tr.appendChild(tdHabitat);

            const tdActions = document.createElement('td');
            const deleteBtn = document.createElement('button');
            deleteBtn.className = 'delete-btn';
            deleteBtn.dataset.id = creature.id;
            deleteBtn.textContent = 'X';
            tdActions.appendChild(deleteBtn);
            tr.appendChild(tdActions);

            creaturesTableBody.appendChild(tr);
        });

    } catch (error) {
        console.error('Error loading creatures:', error);
        creaturesTableBody.innerHTML = '<tr><td colspan="5" style="color: red;">Error loading data. Make sure the Backend is running.</td></tr>';
    }
}

// --- 2. Функція: Додати нового мешканця ---
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

// --- 3. Функція: Видалити мешканця ---
creaturesTableBody.addEventListener('click', async (e) => {
    if (e.target.classList.contains('delete-btn')) {
        const creatureId = e.target.dataset.id;

        if (!confirm(Are you sure you want to delete creature with ID: ${ creatureId }?)) {
    return;
}

try {
    const response = await fetch(${ API_URL } / ${ creatureId }, {
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

// --- Запускаємо завантаження при першому відкритті сторінки ---
document.addEventListener('DOMContentLoaded', loadCreatures);