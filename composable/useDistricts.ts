import { ref } from 'vue'

export function useDistricts() {
  const districts = ref([
    // Москва
    { id: 1, name: 'Центральный', cityId: 1 },
    { id: 2, name: 'Северный', cityId: 1 },
    { id: 3, name: 'Южный', cityId: 1 },
    // Санкт-Петербург
    { id: 4, name: 'Невский', cityId: 2 },
    { id: 5, name: 'Василеостровский', cityId: 2 },
    // Новосибирск
    { id: 6, name: 'Центральный', cityId: 3 },
    { id: 7, name: 'Октябрьский', cityId: 3 },
    // Екатеринбург
    { id: 8, name: 'Ленинский', cityId: 4 },
    { id: 9, name: 'Верх-Исетский', cityId: 4 }
  ])

  const getDistrictsByCity = (cityId) => {
    return districts.value.filter(d => d.cityId === cityId)
  }

  return {
    districts,
    getDistrictsByCity
  }
}