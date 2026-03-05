import { ref } from 'vue'

export function useFormats() {
  const formats = ref([
    { id: 1, name: 'Билборд 3x6', constructionType: 'Billboard' },
    { id: 2, name: 'Билборд 4x8', constructionType: 'Billboard' },
    { id: 3, name: 'Плакат A1', constructionType: 'Poster' },
    { id: 4, name: 'Плакат A0', constructionType: 'Poster' },
    { id: 5, name: 'Ситилайт', constructionType: 'CityLight' },
    { id: 6, name: 'Ситиформат', constructionType: 'CityLight' },
    { id: 7, name: 'Видеоборд 9x16', constructionType: 'Videoboard' },
    { id: 8, name: 'Видеоэкран', constructionType: 'Videoboard' }
  ])

  const getFormatsByType = (constructionType) => {
    return formats.value.filter(f => f.constructionType === constructionType)
  }

  return {
    formats,
    getFormatsByType
  }
}