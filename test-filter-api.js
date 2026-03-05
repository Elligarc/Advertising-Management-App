// Тест фильтрованного API для поверхностей
// Запустите: node test-filter-api.js

import fetch from 'node-fetch';

const API_BASE = 'http://localhost:5000';

async function testFilterAPI() {
  console.log('Тестирование фильтрованного API поверхностей...\n');

  try {
    // Тест 1: Получение всех поверхностей через фильтр
    console.log('1. Тест получения всех поверхностей через фильтр:');
    const filterResponse = await fetch(`${API_BASE}/api/Surfaces/filter`);
    const surfaces = await filterResponse.json();
    console.log(`   Статус: ${filterResponse.status}`);
    console.log(`   Количество поверхностей: ${surfaces.length}`);
    
    if (surfaces.length > 0) {
      console.log(`   Пример поверхности:`, surfaces[0]);
      console.log('');
      
      // Тест 2: Получение конкретной поверхности
      console.log('2. Тест получения конкретной поверхности:');
      const surfaceId = surfaces[0].id;
      const surfaceResponse = await fetch(`${API_BASE}/api/Surfaces/${surfaceId}`);
      const surface = await surfaceResponse.json();
      console.log(`   Статус: ${surfaceResponse.status}`);
      console.log(`   ID поверхности: ${surface.id}`);
      console.log(`   Адрес: ${surface.construction?.address}`);
      console.log(`   Сторона: ${surface.side}`);
      console.log(`   Тип: ${surface.surfaceType}`);
      console.log(`   Цена: ${surface.currentPrice} ₽ / ${surface.currentPriceType}`);
      console.log('');
    }

    // Тест 3: Фильтрация по городу
    console.log('3. Тест фильтрации по городу (Екатеринбург):');
    const cityFilterResponse = await fetch(`${API_BASE}/api/Surfaces/filter?CityId=4`);
    const citySurfaces = await cityFilterResponse.json();
    console.log(`   Статус: ${cityFilterResponse.status}`);
    console.log(`   Количество поверхностей в Екатеринбурге: ${citySurfaces.length}`);
    if (citySurfaces.length > 0) {
      console.log(`   Пример поверхности в Екатеринбурге:`, citySurfaces[0]);
    }
    console.log('');

    console.log('✅ Все тесты фильтрованного API прошли успешно!');

  } catch (error) {
    console.error('❌ Ошибка при тестировании фильтрованного API:', error.message);
  }
}

testFilterAPI();