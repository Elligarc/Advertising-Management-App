// Скрипт для создания тестовой поверхности
// Запустите: node create-test-surface.js

import fetch from 'node-fetch';

const API_BASE = 'http://localhost:5000';

async function createTestSurface() {
  console.log('Создание тестовой поверхности...\n');

  try {
    // Сначала получим список конструкций для выбора
    console.log('1. Получение списка конструкций:');
    const constructionsResponse = await fetch(`${API_BASE}/api/Constructions`);
    const constructions = await constructionsResponse.json();
    console.log(`   Найдено конструкций: ${constructions.length}`);

    if (constructions.length === 0) {
      console.log('❌ Нет доступных конструкций для создания поверхности');
      return;
    }

    const constructionId = constructions[0].id;
    console.log(`   Используем конструкцию ID: ${constructionId}`);
    console.log(`   Адрес: ${constructions[0].address}`);
    console.log('');

    // Создаем тестовую поверхность
    console.log('2. Создание тестовой поверхности:');
    const surfaceData = {
      constructionId: constructionId,
      side: 'A',
      surfaceType: 'Regular',
      initialPrice: 5000,
      priceType: 'PerMonth'
    };

    const createResponse = await fetch(`${API_BASE}/api/Surfaces`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(surfaceData)
    });

    const createdSurface = await createResponse.json();
    
    console.log(`   Статус: ${createResponse.status}`);
    if (createResponse.ok) {
      console.log(`   ✅ Поверхность создана успешно!`);
      console.log(`   ID: ${createdSurface.id}`);
      console.log(`   Сторона: ${createdSurface.side}`);
      console.log(`   Тип: ${createdSurface.surfaceType}`);
      console.log(`   Цена: ${createdSurface.currentPrice} ₽ / ${createdSurface.currentPriceType}`);
    } else {
      console.log(`   ❌ Ошибка создания поверхности:`, createdSurface);
    }

  } catch (error) {
    console.error('❌ Ошибка:', error.message);
  }
}

createTestSurface();