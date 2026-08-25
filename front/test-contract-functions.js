// Тестовая страница для проверки функций договоров
// Запустите эту страницу в браузере для диагностики проблем

console.log('=== Тестирование функций договоров ===')

// Проверка API-базы
const apiBase = 'http://localhost:5000'
console.log('API Base:', apiBase)

// Функция для тестирования получения списка договоров
async function testGetContracts() {
  console.log('=== Тест: Получение списка договоров ===')
  try {
    const response = await fetch(`${apiBase}/api/contracts`)
    console.log('Статус ответа:', response.status)
    console.log('Заголовки ответа:', response.headers)
    
    if (response.ok) {
      const data = await response.json()
      console.log('Данные успешно получены:', data)
      console.log('Количество договоров:', data.length)
      return data
    } else {
      console.error('Ошибка HTTP:', response.status, response.statusText)
      return null
    }
  } catch (error) {
    console.error('Ошибка сети:', error)
    return null
  }
}

// Функция для тестирования получения конкретного договора
async function testGetContract(id) {
  console.log(`=== Тест: Получение договора с ID ${id} ===`)
  try {
    const response = await fetch(`${apiBase}/api/contracts/${id}`)
    console.log('Статус ответа:', response.status)
    
    if (response.ok) {
      const data = await response.json()
      console.log('Данные договора:', data)
      return data
    } else {
      console.error('Ошибка HTTP:', response.status, response.statusText)
      return null
    }
  } catch (error) {
    console.error('Ошибка сети:', error)
    return null
  }
}

// Функция для тестирования создания договора
async function testCreateContract() {
  console.log('=== Тест: Создание договора ===')
  try {
    const testData = {
      clientId: 1 // Предполагаем, что клиент с ID 1 существует
    }
    
    const response = await fetch(`${apiBase}/api/contracts`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(testData)
    })
    
    console.log('Статус ответа:', response.status)
    
    if (response.ok) {
      const data = await response.json()
      console.log('Договор успешно создан:', data)
      return data
    } else {
      const errorData = await response.json()
      console.error('Ошибка создания:', response.status, errorData)
      return null
    }
  } catch (error) {
    console.error('Ошибка сети:', error)
    return null
  }
}

// Функция для тестирования добавления поверхности к договору
async function testAddSurfaceToContract(contractId, surfaceId) {
  console.log(`=== Тест: Добавление поверхности ${surfaceId} к договору ${contractId} ===`)
  try {
    const testData = {
      surfaceId: surfaceId,
      startDate: '2024-01-01',
      endDate: '2024-12-31',
      price: 1000,
      priceType: 'PerShow'
    }
    
    const response = await fetch(`${apiBase}/api/contracts/${contractId}/items`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(testData)
    })
    
    console.log('Статус ответа:', response.status)
    
    if (response.ok) {
      const data = await response.json()
      console.log('Поверхность успешно добавлена:', data)
      return data
    } else {
      const errorData = await response.json()
      console.error('Ошибка добавления:', response.status, errorData)
      return null
    }
  } catch (error) {
    console.error('Ошибка сети:', error)
    return null
  }
}

// Функция для тестирования удаления договора
async function testDeleteContract(id) {
  console.log(`=== Тест: Удаление договора с ID ${id} ===`)
  try {
    const response = await fetch(`${apiBase}/api/contracts/${id}`, {
      method: 'DELETE'
    })
    
    console.log('Статус ответа:', response.status)
    
    if (response.ok) {
      console.log('Договор успешно удален')
      return true
    } else {
      const errorData = await response.json()
      console.error('Ошибка удаления:', response.status, errorData)
      return false
    }
  } catch (error) {
    console.error('Ошибка сети:', error)
    return false
  }
}

// Основная функция тестирования
async function runTests() {
  console.log('=== Запуск тестов ===')
  
  // 1. Проверка получения списка договоров
  const contracts = await testGetContracts()
  
  if (contracts && contracts.length > 0) {
    const firstContract = contracts[0]
    console.log('=== Используем первый договор для тестов ===')
    console.log('ID первого договора:', firstContract.id)
    
    // 2. Проверка получения конкретного договора
    await testGetContract(firstContract.id)
    
    // 3. Проверка добавления поверхности (если есть доступные поверхности)
    // Для этого нужно сначала получить список поверхностей
    try {
      const surfacesResponse = await fetch(`${apiBase}/api/surfaces`)
      if (surfacesResponse.ok) {
        const surfaces = await surfacesResponse.json()
        if (surfaces.length > 0) {
          await testAddSurfaceToContract(firstContract.id, surfaces[0].id)
        }
      }
    } catch (error) {
      console.error('Не удалось получить список поверхностей:', error)
    }
    
    // 4. Проверка удаления договора (только для тестовых договоров!)
    // ВНИМАНИЕ: Это удалит реальный договор!
    // await testDeleteContract(firstContract.id)
  }
  
  // 5. Проверка создания нового договора
  await testCreateContract()
  
  console.log('=== Тесты завершены ===')
}

// Запуск тестов при загрузке страницы
document.addEventListener('DOMContentLoaded', () => {
  console.log('=== Страница загружена, запускаем тесты ===')
  runTests()
})

// Экспорт функций для ручного тестирования в консоли
window.contractTests = {
  testGetContracts,
  testGetContract,
  testCreateContract,
  testAddSurfaceToContract,
  testDeleteContract,
  runTests
}

console.log('=== Функции тестирования доступны в window.contractTests ===')