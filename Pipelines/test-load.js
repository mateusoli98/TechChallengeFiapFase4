import http from 'k6/http';
import { check } from 'k6';

function generatePhone() {
    return '9' + Math.floor(Math.random() * 1000000000); // Gera um número de 9 dígitos começando com 9
}

function generateEmail() {
    return `user${Math.floor(Math.random() * 10000)}@example.com`; // Gera um email aleatório
}

function generateName() {
    const names = ["Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hannah", "Isaac", "Jack"];
    return names[Math.floor(Math.random() * names.length)]; // Gera um nome aleatório da lista
}

export default function () {
    const url = 'http://localhost:30000/contact';
    
    const payload = JSON.stringify({
        areaCode: 12,
        email: generateEmail(),
        name: generateName(),
        phone: generatePhone()
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    let res = http.post(url, payload, params);

    check(res, {
        'is status 200': (r) => r.status === 200,
    });
}
