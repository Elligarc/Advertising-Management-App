// Простой тест для проверки API-вызовов
// Запустите: node test-api.js

import fetch from 'node-fetch';

const API_BASE = 'http://localhost:5000';

async function testAPI() {
  console.log('Тестирование API поверхностей...\n');

  try {
    // Тест 1: Получение списка поверхностей
    console.log('1. Тест получения списка поверхностей:');
    const surfacesResponse = await fetch(`${API_BASE}/api/Surfaces`);
    const surfaces = await surfacesResponse.json();
    console.log(`   Статус: ${surfacesResponse.status}`);
    console.log(`   Количество поверхностей: ${surfaces.length}`);
    if (surfaces.length > 0) {
      console.log(`   Пример поверхности:`, surfaces[0]);
    }
    console.log('');

    // Тест 2: Получение конкретной поверхности
    if (surfaces.length > 0) {
      console.log('2. Тест получения конкретной поверхности:');
      const surfaceId = surfaces[0].id;
      const surfaceResponse = await fetch(`${API_BASE}/api/Surfaces/${surfaceId}`);
      const surface = await surfaceResponse.json();
      console.log(`   Статус: ${surfaceResponse.status}`);
      console.log(`   ID поверхности: ${surface.id}`);
      console.log(`   Адрес: ${surface.construction?.address}`);
      console.log('');
    }

    // Тест 3: Получение списка конструкций
    console.log('3. Тест получения списка конструкций:');
    const constructionsResponse = await fetch(`${API_BASE}/api/Constructions`);
    const constructions = await constructionsResponse.json();
    console.log(`   Статус: ${constructionsResponse.status}`);
    console.log(`   Количество конструкций: ${constructions.length}`);
    if (constructions.length > 0) {
      console.log(`   Пример конструкции:`, constructions[0]);
    }
    console.log('');

    console.log('✅ Все тесты API прошли успешно!');

  } catch (error) {
    console.error('❌ Ошибка при тестировании API:', error.message);
  }
}

testAPI();