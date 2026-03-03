<template>
  <div class="calendar-page">
    <div class="page-header">
      <h1 class="page-title">Календарь аренд</h1>
      <div class="calendar-nav">
        <button class="btn" @click="prevMonth">←</button>
        <span class="current-month">{{ monthTitle }}</span>
        <button class="btn" @click="nextMonth">→</button>
      </div>
    </div>

    <div class="calendar-grid">
      <div class="weekdays">
        <div v-for="day in weekDays" :key="day" class="weekday">{{ day }}</div>
      </div>
      <div class="days">
        <div
          v-for="(day, index) in calendarDays"
          :key="index"
          class="day-cell"
          :class="{ otherMonth: !day.currentMonth, today: day.isToday }"
        >
          <span class="day-num">{{ day.date.getDate() }}</span>
          <div class="day-events">
            <div
              v-for="event in day.events"
              :key="event.id"
              class="event-pill"
              :class="event.type"
              :title="event.title"
            >
              {{ event.short }}
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="legend">
      <span class="legend-item"><span class="dot start"></span> Начало аренды</span>
      <span class="legend-item"><span class="dot end"></span> Окончание аренды</span>
      <span class="legend-item"><span class="dot repair"></span> Ремонт / простой</span>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const weekDays = ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс']

const currentDate = ref(new Date(2026, 2, 1)) // март 2026

const monthTitle = computed(() => {
  return currentDate.value.toLocaleDateString('ru-RU', { month: 'long', year: 'numeric' })
})

// События для отображения (тестовые)
const events = ref([
  { id: 1, date: '2026-03-15', type: 'end', title: 'Окончание: Билборд Тверская 15 — ООО Ромашка', short: 'Конец' },
  { id: 2, date: '2026-03-20', type: 'start', title: 'Начало: Ситилайт Арбат 10 — ТехноПлюс', short: 'Старт' },
  { id: 3, date: '2026-03-10', type: 'repair', title: 'Ремонт: Видеоэкран Садовая 5', short: 'Ремонт' },
  { id: 4, date: '2026-03-01', type: 'start', title: 'Начало: Билборд Ленинский 20', short: 'Старт' },
  { id: 5, date: '2026-03-22', type: 'end', title: 'Окончание: Баннер МКАД 45 км', short: 'Конец' }
])

const calendarDays = computed(() => {
  const year = currentDate.value.getFullYear()
  const month = currentDate.value.getMonth()
  const first = new Date(year, month, 1)
  const last = new Date(year, month + 1, 0)
  const startPad = (first.getDay() + 6) % 7
  const daysInMonth = last.getDate()
  const prevMonthDays = new Date(year, month, 0).getDate()
  const today = new Date()
  today.setHours(0, 0, 0, 0)

  const result = []

  for (let i = 0; i < startPad; i++) {
    const d = new Date(year, month - 1, prevMonthDays - startPad + i + 1)
    result.push({
      date: d,
      currentMonth: false,
      isToday: false,
      events: getEventsForDate(d)
    })
  }

  for (let d = 1; d <= daysInMonth; d++) {
    const date = new Date(year, month, d)
    const isToday = date.getTime() === today.getTime()
    result.push({
      date,
      currentMonth: true,
      isToday,
      events: getEventsForDate(date)
    })
  }

  const remaining = 42 - result.length
  for (let i = 1; i <= remaining; i++) {
    const date = new Date(year, month + 1, i)
    result.push({
      date,
      currentMonth: false,
      isToday: false,
      events: getEventsForDate(date)
    })
  }

  return result
})

function getEventsForDate(date) {
  const y = date.getFullYear()
  const m = String(date.getMonth() + 1).padStart(2, '0')
  const d = String(date.getDate()).padStart(2, '0')
  const str = `${y}-${m}-${d}`
  return events.value.filter(e => e.date === str)
}

function prevMonth() {
  currentDate.value = new Date(currentDate.value.getFullYear(), currentDate.value.getMonth() - 1, 1)
}

function nextMonth() {
  currentDate.value = new Date(currentDate.value.getFullYear(), currentDate.value.getMonth() + 1, 1)
}
</script>

<style scoped>
.calendar-page {
  animation: fadeIn 0.5s;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 2rem;
}

.page-title {
  font-size: 2rem;
  color: #2c3e50;
}

.calendar-nav {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.current-month {
  font-size: 1.25rem;
  font-weight: 600;
  color: #2c3e50;
  min-width: 200px;
  text-align: center;
}

.calendar-grid {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.weekdays {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 4px;
  margin-bottom: 8px;
}

.weekday {
  text-align: center;
  font-weight: 600;
  color: #7f8c8d;
  font-size: 0.9rem;
}

.days {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 4px;
}

.day-cell {
  min-height: 100px;
  border: 1px solid #e9ecef;
  border-radius: 8px;
  padding: 8px;
  background: #fafafa;
}

.day-cell.otherMonth {
  background: #f0f2f5;
  opacity: 0.7;
}

.day-cell.today {
  border-color: #2a5298;
  background: #e8eef7;
}

.day-num {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 4px;
  display: block;
}

.day-events {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.event-pill {
  font-size: 0.75rem;
  padding: 2px 6px;
  border-radius: 4px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.event-pill.start {
  background: #d4edda;
  color: #155724;
}

.event-pill.end {
  background: #f8d7da;
  color: #721c24;
}

.event-pill.repair {
  background: #fff3cd;
  color: #856404;
}

.legend {
  margin-top: 1.5rem;
  display: flex;
  gap: 2rem;
  flex-wrap: wrap;
  color: #7f8c8d;
  font-size: 0.9rem;
}

.dot {
  display: inline-block;
  width: 12px;
  height: 12px;
  border-radius: 4px;
  margin-right: 0.5rem;
  vertical-align: middle;
}

.dot.start { background: #d4edda; }
.dot.end { background: #f8d7da; }
.dot.repair { background: #fff3cd; }

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
