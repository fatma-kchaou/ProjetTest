// tests/api-login.spec.js
const { test, expect } = require('@playwright/test');

const API_BASE = 'http://localhost:5041';   // change 5231 par ton port si différent

test.describe('API Account - Login', () => {

    test('TC019 - Login réussi → token JWT retourné', async ({ request }) => {
        const response = await request.post(`${API_BASE}/api/Account/login`, {
            headers: { 'Content-Type': 'application/json' },
            data: {
                username: 'Fatma',
                password: 'Fatma123'
            }
        });

        expect(response.status()).toBe(200);
        const body = await response.json();
        expect(body.token).toBeDefined();
        console.log('Token reçu :', body.token.substring(0, 30) + '...');
    });

    test('TC004 - Login échoué (mauvais mot de passe) → 401', async ({ request }) => {
        const response = await request.post(`${API_BASE}/api/Account/login`, {
            headers: { 'Content-Type': 'application/json' },
            data: {
                username: 'Fatma',
                password: 'mauvaismotdepasse'
            }
        });

        expect(response.status()).toBe(401);
        const text = await response.text();
        expect(text).toContain('Invalid credentials');
    });

    test('TC004-bis - Login utilisateur inexistant → 401', async ({ request }) => {
        const response = await request.post(`${API_BASE}/api/Account/login`, {
            data: { username: 'Inexistant123', password: 'whatever' }
        });
        expect(response.status()).toBe(401);
    });
});