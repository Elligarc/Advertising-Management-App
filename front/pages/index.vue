<template>
  <div class="dashboard">
    <!-- Финансовая статистика -->
    <div class="stats-grid finance-stats">
      <div class="stat-card primary">
        <div class="stat-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
            <path d="M11.8 10.9c-2.27-.59-3-1.2-3-2.15 0-1.09 1.01-1.8 2.2-1.8.92 0 2.4.8 2.4 2.65 0 1.25-.8 1.93-1.75 2.34-.47.21-.5 1.48-.5 1.48s1.53.2 2.45.57c1.64.66 1.87 1.71 1.87 3.11 0 2.42-2.35 4.19-5.75 4.19-3.4 0-6.25-1.78-6.25-4.19 0-2.55 2.45-4.8 5.8-4.8zm0-1.8c-2.8 0-4.9 1.45-4.9 3.2 0 1.4 1.26 2.55 3.1 2.55 1.8 0 2.95-.9 2.95-2.3 0-1.75-2.1-3.45-5.15-3.45zm-2.6 10.1c1 0 1.8.8 1.8 1.8s-.8 1.8-1.8 1.8-1.8-.8-1.8-1.8.8-1.8 1.8-1.8zm3.6-.8c-1.4 1.4-1.4 3.6 0 5 1.4 1.4 3.6 1.4 5 0 1.4-1.4 1.4-3.6 0-5-1.4-1.4-3.6-1.4-5 0z" fill="currentColor"/>
          </svg>
        </div>
        <div class="stat-content">
          <span class="stat-label">Оборот за месяц</span>
          <span class="stat-value">2 450 000 ₽</span>
          <span class="stat-change positive">+12.5%</span>
        </div>
      </div>
      
      <div class="stat-card danger">
        <div class="stat-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z" fill="currentColor"/>
          </svg>
        </div>
        <div class="stat-content">
          <span class="stat-label">Просроченные платежи</span>
          <span class="stat-value">156 500 ₽</span>
          <span class="stat-change negative">3 контракта</span>
        </div>
      </div>
      
      <div class="stat-card success">
        <div class="stat-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
            <path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2zm0 3.16L14.1 9.5l4.63.67-3.36 3.27 1 4.66L12 15.6l-4.77 2.5 1-4.66L5.87 10.17l4.63-.67L12 5.16z" fill="currentColor"/>
          </svg>
        </div>
        <div class="stat-content">
          <span class="stat-label">Средняя стоимость</span>
          <span class="stat-value">45 200 ₽/мес</span>
          <span class="stat-change positive">+8.3%</span>
        </div>
      </div>
      
      <div class="stat-card warning">
        <div class="stat-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z" fill="currentColor"/>
          </svg>
        </div>
        <div class="stat-content">
          <span class="stat-label">Коэффициент заполняемости</span>
          <span class="stat-value">76%</span>
          <span class="stat-change neutral">+2.1%</span>
        </div>
      </div>
    </div>

    <!-- Основная статистика -->
    <div class="stats-grid main-stats">
      <div class="stat-card primary">
        <div class="stat-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
            <path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z" fill="currentColor"/>
          </svg>
        </div>
        <div class="stat-content">
          <span class="stat-label">Всего поверхностей</span>
          <span class="stat-value">{{ totalSurfaces }}</span>
        </div>
      </div>
      
      <div class="stat-card success">
        <div class="stat-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z" fill="currentColor"/>
          </svg>
        </div>
        <div class="stat-content">
          <span class="stat-label">Свободны</span>
          <span class="stat-value">{{ availableSurfaces }}</span>
        </div>
      </div>
      
      <div class="stat-card danger">
        <div class="stat-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z" fill="currentColor"/>
          </svg>
        </div>
        <div class="stat-content">
          <span class="stat-label">Заняты</span>
          <span class="stat-value">{{ occupiedSurfaces }}</span>
        </div>
      </div>
      
      <div class="stat-card warning">
        <div class="stat-icon">
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
            <path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z" fill="currentColor"/>
          </svg>
        </div>
        <div class="stat-content">
          <span class="stat-label">В простое</span>
          <span class="stat-value">{{ downtimeSurfaces }}</span>
        </div>
      </div>
    </div>

    <!-- Быстрые действия -->
    <div class="quick-actions">
      <h2 class="section-title">Быстрые действия</h2>
      <div class="actions-grid">
        <NuxtLink to="/surfaces/add" class="action-card primary">
          <div class="action-icon">
            <svg width="32" height="32" viewBox="0 0 24 24" fill="none">
              <path d="M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z" fill="currentColor"/>
            </svg>
          </div>
          <div class="action-content">
            <h3>Добавить поверхность</h3>
            <p>Зарегистрируйте новую рекламную поверхность</p>
          </div>
        </NuxtLink>
        
        <NuxtLink to="/clients/add" class="action-card success">
          <div class="action-icon">
            <svg width="32" height="32" viewBox="0 0 24 24" fill="none">
              <path d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z" fill="currentColor"/>
            </svg>
          </div>
          <div class="action-content">
            <h3>Новый клиент</h3>
            <p>Зарегистрируйте нового клиента</p>
          </div>
        </NuxtLink>
        
        <NuxtLink to="/contracts/add" class="action-card warning">
          <div class="action-icon">
            <svg width="32" height="32" viewBox="0 0 24 24" fill="none">
              <path d="M14 2H6c-1.1 0-1.99.9-1.99 2L4 20c0 1.1.89 2 1.99 2H18c1.1 0 2-.9 2-2V8l-6-6zm2 16H8v-2h8v2zm0-4H8v-2h8v2zm-3-5V3.5L18.5 9H13z" fill="currentColor"/>
            </svg>
          </div>
          <div class="action-content">
            <h3>Новая аренда</h3>
            <p>Оформите договор аренды</p>
          </div>
        </NuxtLink>
        
        <NuxtLink to="/downtime/add" class="action-card danger">
          <div class="action-icon">
            <svg width="32" height="32" viewBox="0 0 24 24" fill="none">
              <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z" fill="currentColor"/>
            </svg>
          </div>
          <div class="action-content">
            <h3>Отметить простой</h3>
            <p>Зарегистрируйте ремонт или простой</p>
          </div>
        </NuxtLink>
      </div>
    </div>

    <!-- Основной контент и боковая панель -->
    <div class="dashboard-layout">
      <!-- Левая колонка -->
      <div class="main-content">
        <!-- Ближайшие события -->
        <div class="section events-section">
          <div class="section-header">
            <h2 class="section-title">Ближайшие события</h2>
            <div class="events-filters">
              <button class="filter-btn active" @click="filterEvents('all')">Все</button>
              <button class="filter-btn" @click="filterEvents('rental')">Аренда</button>
              <button class="filter-btn" @click="filterEvents('repair')">Ремонт</button>
              <button class="filter-btn" @click="filterEvents('payment')">Платежи</button>
            </div>
          </div>
          <div class="events-list">
            <div class="event-item" v-for="event in filteredEvents" :key="event.id" :class="event.type">
              <div class="event-icon">
                <svg v-if="event.type === 'warning'" width="24" height="24" viewBox="0 0 24 24" fill="none">
                  <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z" fill="currentColor"/>
                </svg>
                <svg v-else-if="event.type === 'success'" width="24" height="24" viewBox="0 0 24 24" fill="none">
                  <path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2zm0 3.16L14.1 9.5l4.63.67-3.36 3.27 1 4.66L12 15.6l-4.77 2.5 1-4.66L5.87 10.17l4.63-.67L12 5.16z" fill="currentColor"/>
                </svg>
                <svg v-else width="24" height="24" viewBox="0 0 24 24" fill="none">
                  <path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2zm0 3.16L14.1 9.5l4.63.67-3.36 3.27 1 4.66L12 15.6l-4.77 2.5 1-4.66L5.87 10.17l4.63-.67L12 5.16z" fill="currentColor"/>
                </svg>
              </div>
              <div class="event-main">
                <div class="event-header">
                  <span class="event-title">{{ event.title }}</span>
                  <span class="event-time">{{ formatTime(event.date) }}</span>
                </div>
                <div class="event-details">
                  <span class="event-desc">{{ event.description }}</span>
                  <div class="event-meta">
                    <span class="event-location">{{ event.location }}</span>
                    <span class="event-client">{{ event.client }}</span>
                  </div>
                </div>
              </div>
              <div class="event-actions">
                <span class="event-badge" :class="event.type">{{ event.badgeText }}</span>
                <button class="event-btn" v-if="event.action">{{ event.action }}</button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Правая колонка -->
      <div class="sidebar">
        <!-- Статистика по типам поверхностей -->
        <div class="section sidebar-stats">
          <h2 class="section-title">Статистика по типам</h2>
          <div class="type-stats">
            <div class="type-stat" v-for="type in surfaceTypes" :key="type.name">
              <div class="type-icon" :style="{ backgroundColor: type.color }"></div>
              <div class="type-content">
                <h3>{{ type.name }}</h3>
                <div class="type-progress">
                  <div class="progress-bar">
                    <div class="progress" :style="{ width: type.percentage + '%', backgroundColor: type.color }"></div>
                  </div>
                  <span class="type-value">{{ type.count }} / {{ type.total }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Дополнительная аналитика -->
        <div class="section analytics-section">
          <div class="analytics-grid">
            <div class="analytics-card">
              <h3 class="analytics-title">Загруженность по районам</h3>
              <div class="analytics-list">
                <div class="analytics-item" v-for="district in districtStats" :key="district.name">
                  <div class="analytics-info">
                    <span class="analytics-label">{{ district.name }}</span>
                    <span class="analytics-percent">{{ district.load }}%</span>
                  </div>
                  <div class="analytics-bar">
                    <div class="analytics-fill" :style="{ width: district.load + '%', backgroundColor: district.color }"></div>
                  </div>
                </div>
              </div>
            </div>

            <div class="analytics-card">
              <h3 class="analytics-title">Динамика за неделю</h3>
              <div class="trend-chart">
                <div class="trend-item" v-for="day in weekTrends" :key="day.label">
                  <span class="trend-label">{{ day.label }}</span>
                  <div class="trend-bar" :style="{ height: day.value + '%' }"></div>
                  <span class="trend-value">{{ day.value }}%</span>
                </div>
              </div>
            </div>

            <div class="analytics-card">
              <h3 class="analytics-title">Эффективность</h3>
              <div class="efficiency-grid">
                <div class="efficiency-item">
                  <div class="efficiency-value">{{ efficiencyStats.avgRentDuration }} дн</div>
                  <div class="efficiency-label">Средний срок аренды</div>
                </div>
                <div class="efficiency-item">
                  <div class="efficiency-value">{{ efficiencyStats.downtimePercent }}%</div>
                  <div class="efficiency-label">Простой</div>
                </div>
                <div class="efficiency-item">
                  <div class="efficiency-value">{{ efficiencyStats.renewalRate }}%</div>
                  <div class="efficiency-label">Продление</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const totalSurfaces = 45
const availableSurfaces = 18
const occupiedSurfaces = 21
const downtimeSurfaces = 6

const upcomingEvents = [
  {
    id: 1,
    date: new Date(2026, 2, 20, 18, 0),
    title: 'Окончание аренды',
    description: 'Билборд Тверская 15 - ООО Ромашка',
    type: 'warning',
    badgeText: 'Осталось 2 дня',
    location: 'Тверская 15',
    client: 'ООО Ромашка',
    action: 'Продлить'
  },
  {
    id: 2,
    date: new Date(2026, 2, 22, 10, 0),
    title: 'Начало аренды',
    description: 'Ситилайт Арбат 10 - ТехноПлюс',
    type: 'success',
    badgeText: 'Новая аренда',
    location: 'Арбат 10',
    client: 'ТехноПлюс',
    action: 'Просмотреть'
  },
  {
    id: 3,
    date: new Date(2026, 2, 25, 14, 0),
    title: 'Окончание ремонта',
    description: 'Видеоэкран Садовая 5',
    type: 'repair',
    badgeText: 'В ремонте',
    location: 'Садовая 5',
    client: 'Техобслуживание',
    action: 'Проверить'
  },
  {
    id: 4,
    date: new Date(2026, 2, 21, 16, 30),
    title: 'Просроченный платеж',
    description: 'Контракт №1234 - ООО СтройГрад',
    type: 'payment',
    badgeText: '15 дней',
    location: 'Центральный офис',
    client: 'ООО СтройГрад',
    action: 'Напомнить'
  },
  {
    id: 5,
    date: new Date(2026, 2, 23, 9, 0),
    title: 'Плановый осмотр',
    description: 'Билборд Ленинский 20',
    type: 'maintenance',
    badgeText: 'Через 3 дня',
    location: 'Ленинский 20',
    client: 'Технический отдел',
    action: 'Запланировать'
  },
  {
    id: 6,
    date: new Date(2026, 2, 24, 11, 15),
    title: 'Завершение кампании',
    description: 'Рекламная кампания "Весенняя распродажа"',
    type: 'success',
    badgeText: '1 день',
    location: 'Все поверхности',
    client: 'Магазин "Уют"',
    action: 'Отчет'
  },
  {
    id: 7,
    date: new Date(2026, 2, 26, 13, 45),
    title: 'Техническое обслуживание',
    description: 'Видеоэкран Красная площадь',
    type: 'maintenance',
    badgeText: 'Через 6 дней',
    location: 'Красная площадь',
    client: 'Техобслуживание',
    action: 'Подготовить'
  },
  {
    id: 8,
    date: new Date(2026, 2, 27, 15, 0),
    title: 'Подписание контракта',
    description: 'Новая аренда - Билборд Тверская 15',
    type: 'success',
    badgeText: 'Через 7 дней',
    location: 'Тверская 15',
    client: 'Новый клиент',
    action: 'Подготовить'
  }
]

const filter = ref('all')

const filteredEvents = computed(() => {
  if (filter.value === 'all') return upcomingEvents
  return upcomingEvents.filter(event => event.type === filter.value)
})

function filterEvents(type) {
  filter.value = type
}

function formatTime(date) {
  return date.toLocaleDateString('ru-RU', { 
    day: '2-digit', 
    month: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const surfaceTypes = [
  { name: 'Билборды', count: 15, total: 20, percentage: 75, color: '#1e3c72' },
  { name: 'Ситилайты', count: 12, total: 15, percentage: 80, color: '#2a5298' },
  { name: 'Видеоэкраны', count: 8, total: 10, percentage: 80, color: '#1a3361' }
]

function formatDate(date) {
  return date.toLocaleDateString('ru-RU', { day: '2-digit', month: '2-digit' })
}

const districtStats = [
  { name: 'Центральный', load: 85, color: '#1e3c72' },
  { name: 'Советский', load: 72, color: '#2a5298' },
  { name: 'Ленинский', load: 68, color: '#1a3361' },
  { name: 'Кировский', load: 91, color: '#152646' },
  { name: 'Октябрьский', load: 76, color: '#23427a' }
]

const weekTrends = [
  { label: 'Пн', value: 65 },
  { label: 'Вт', value: 72 },
  { label: 'Ср', value: 78 },
  { label: 'Чт', value: 82 },
  { label: 'Пт', value: 88 },
  { label: 'Сб', value: 85 },
  { label: 'Вс', value: 75 }
]

const efficiencyStats = {
  avgRentDuration: 45,
  downtimePercent: 12,
  renewalRate: 68
}
</script>

<style scoped>
.dashboard {
  animation: fadeIn 0.5s;
}

/* Hero секция */
.hero-section {
  background: linear-gradient(135deg, #1e3c72 0%, #2a5298 100%);
  color: white;
  border-radius: 20px;
  padding: 3rem;
  margin-bottom: 2rem;
  position: relative;
  overflow: hidden;
}

.hero-section::before {
  content: '';
  position: absolute;
  top: 0;
  right: 0;
  width: 200px;
  height: 200px;
  background: rgba(255,255,255,0.1);
  border-radius: 50%;
  transform: translate(50%, -50%);
}

.hero-content {
  position: relative;
  z-index: 2;
}

.hero-title {
  font-size: 2.5rem;
  font-weight: 800;
  margin: 0 0 0.5rem 0;
  line-height: 1.2;
}

.hero-subtitle {
  font-size: 1.1rem;
  opacity: 0.9;
  margin: 0;
  font-weight: 400;
}

.hero-stats {
  display: flex;
  gap: 2rem;
  margin-top: 2rem;
  position: relative;
  z-index: 2;
}

.stat-item {
  text-align: center;
  padding: 1rem;
  background: rgba(255,255,255,0.1);
  border-radius: 12px;
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255,255,255,0.2);
}

.stat-number {
  font-size: 2rem;
  font-weight: 700;
  display: block;
}

.stat-label {
  font-size: 0.9rem;
  opacity: 0.8;
  margin-top: 0.25rem;
}

/* Статистика */
.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 3rem;
}

.stat-card {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  display: flex;
  align-items: center;
  gap: 1.5rem;
  box-shadow: 0 10px 30px rgba(0,0,0,0.1);
  transition: all 0.3s ease;
  border: 1px solid #e5e7eb;
}

.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 15px 40px rgba(0,0,0,0.15);
  border-color: #cbd5e1;
}

.stat-card.primary .stat-icon svg path { fill: #1e3c72; }
.stat-card.success .stat-icon svg path { fill: #27ae60; }
.stat-card.danger .stat-icon svg path { fill: #e74c3c; }
.stat-card.warning .stat-icon svg path { fill: #f1c40f; }

.stat-icon {
  width: 48px;
  height: 48px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #f8fafc;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
  flex-shrink: 0;
}

.stat-content {
  display: flex;
  flex-direction: column;
}

.stat-label {
  color: #6b7280;
  font-size: 0.9rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  margin-bottom: 0.25rem;
}

.stat-value {
  font-size: 2.5rem;
  font-weight: 800;
  color: #1a1a2e;
  line-height: 1;
}

/* Быстрые действия */
.quick-actions {
  margin-bottom: 3rem;
}

.section-title {
  font-size: 1.5rem;
  color: #1a1a2e;
  margin-bottom: 1.5rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 700;
}

.actions-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
}

.action-card {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  padding: 2rem;
  background: white;
  border-radius: 16px;
  text-decoration: none;
  color: inherit;
  box-shadow: 0 10px 30px rgba(0,0,0,0.1);
  transition: all 0.3s ease;
  border: 1px solid #e5e7eb;
}

.action-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 15px 40px rgba(0,0,0,0.15);
  border-color: #cbd5e1;
}

.action-card.primary { border-left: 4px solid #1e3c72; }
.action-card.success { border-left: 4px solid #27ae60; }
.action-card.warning { border-left: 4px solid #f1c40f; }
.action-card.danger { border-left: 4px solid #e74c3c; }

.action-icon {
  width: 64px;
  height: 64px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #f8fafc;
  border-radius: 16px;
  border: 1px solid #e5e7eb;
  flex-shrink: 0;
}

.action-card:hover .action-icon {
  background: #e8eef7;
  border-color: #cbd5e1;
}

.action-content h3 {
  margin: 0 0 0.25rem 0;
  font-size: 1.25rem;
  font-weight: 700;
  color: #1a1a2e;
}

.action-content p {
  margin: 0;
  color: #6b7280;
  font-size: 0.9rem;
}

/* События */
.section {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  margin-bottom: 2rem;
  box-shadow: 0 10px 30px rgba(0,0,0,0.1);
  border: 1px solid #e5e7eb;
}

.events-section {
  margin-bottom: 3rem;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.events-filters {
  display: flex;
  gap: 0.5rem;
}

.filter-btn {
  padding: 0.5rem 1rem;
  border: 1px solid #e5e7eb;
  background: white;
  border-radius: 8px;
  cursor: pointer;
  font-size: 0.9rem;
  font-weight: 600;
  color: #6b7280;
  transition: all 0.3s ease;
}

.filter-btn:hover {
  background: #f8fafc;
  border-color: #cbd5e1;
}

.filter-btn.active {
  background: #1e3c72;
  color: white;
  border-color: #1e3c72;
}

.events-list {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.event-item {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  padding: 1.5rem;
  background: #f8fafc;
  border-radius: 12px;
  transition: all 0.3s ease;
  border: 1px solid #e5e7eb;
  min-height: 120px;
}

.event-item:hover {
  background: #f1f5f9;
  transform: translateX(5px);
  border-color: #cbd5e1;
}

.event-icon {
  width: 48px;
  height: 48px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: white;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
  flex-shrink: 0;
}

.event-icon svg path {
  transition: fill 0.3s ease;
}

.event-item.warning .event-icon svg path { fill: #f1c40f; }
.event-item.success .event-icon svg path { fill: #27ae60; }
.event-item.repair .event-icon svg path { fill: #e74c3c; }
.event-item.maintenance .event-icon svg path { fill: #9b59b6; }
.event-item.payment .event-icon svg path { fill: #e67e22; }

.event-main {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.event-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.event-title {
  font-weight: 700;
  color: #1a1a2e;
  font-size: 1.1rem;
}

.event-time {
  font-size: 0.9rem;
  color: #6b7280;
  background: white;
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  border: 1px solid #e5e7eb;
}

.event-details {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.event-desc {
  font-size: 1rem;
  color: #1a1a2e;
  font-weight: 600;
}

.event-meta {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.event-location, .event-client {
  font-size: 0.85rem;
  color: #6b7280;
  background: white;
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  border: 1px solid #e5e7eb;
}

.event-actions {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  align-items: flex-end;
  flex-shrink: 0;
}

.event-badge {
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  border: 1px solid rgba(0,0,0,0.1);
}

.event-badge.warning {
  background: linear-gradient(135deg, #fff3cd 0%, #ffeaa7 100%);
  color: #856404;
  border-color: #ffd8a0;
}

.event-badge.success {
  background: linear-gradient(135deg, #d4edda 0%, #c3e6cb 100%);
  color: #155724;
  border-color: #b8dacc;
}

.event-badge.repair {
  background: linear-gradient(135deg, #f8d7da 0%, #f5c6cb 100%);
  color: #721c24;
  border-color: #f1b0b7;
}

.event-badge.payment {
  background: linear-gradient(135deg, #ffeaa7 0%, #fdcb6e 100%);
  color: #856404;
  border-color: #f39c12;
}

.event-badge.maintenance {
  background: linear-gradient(135deg, #dcd0ff 0%, #b8a1ff 100%);
  color: #4a148c;
  border-color: #9b59b6;
}

.event-btn {
  padding: 0.5rem 1rem;
  background: #1e3c72;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
}

.event-btn:hover {
  background: #152646;
  transform: translateY(-1px);
}

/* Статистика по типам */
.type-stats {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 1.5rem;
}

.type-stat {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  padding: 1.5rem;
  background: #f8fafc;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
  transition: all 0.3s ease;
}

.type-stat:hover {
  background: #f1f5f9;
  transform: translateX(5px);
  border-color: #cbd5e1;
}

.type-icon {
  width: 16px;
  height: 16px;
  border-radius: 4px;
  flex-shrink: 0;
}

.type-content {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.type-content h3 {
  margin: 0 0 0.5rem 0;
  font-size: 1.1rem;
  font-weight: 700;
  color: #1a1a2e;
}

.type-progress {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.progress-bar {
  flex: 1;
  height: 12px;
  background: #e5e7eb;
  border-radius: 999px;
  overflow: hidden;
  position: relative;
}

.progress {
  height: 100%;
  border-radius: 999px;
  position: relative;
  overflow: hidden;
}

.progress::after {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(90deg, transparent, rgba(255,255,255,0.3), transparent);
  animation: shimmer 2s infinite;
}

.type-value {
  font-weight: 700;
  color: #1a1a2e;
  font-variant-numeric: tabular-nums;
  min-width: 60px;
  text-align: right;
}

/* Аналитика */
.analytics-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 1.5rem;
}

.analytics-card {
  background: #f8fafc;
  border-radius: 12px;
  padding: 1.5rem;
  border: 1px solid #e5e7eb;
  transition: all 0.3s ease;
}

.analytics-card:hover {
  background: #f1f5f9;
  transform: translateY(-2px);
  border-color: #cbd5e1;
}

.analytics-title {
  font-size: 1.1rem;
  font-weight: 700;
  color: #1a1a2e;
  margin: 0 0 1.5rem 0;
}

.analytics-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.analytics-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
}

.analytics-info {
  display: flex;
  align-items: center;
  gap: 1rem;
  flex: 1;
}

.analytics-label {
  font-size: 0.9rem;
  color: #6b7280;
  font-weight: 600;
}

.analytics-percent {
  font-size: 1rem;
  font-weight: 800;
  color: #1a1a2e;
}

.analytics-bar {
  flex: 1;
  height: 8px;
  background: #e5e7eb;
  border-radius: 999px;
  overflow: hidden;
  position: relative;
}

.analytics-fill {
  height: 100%;
  border-radius: 999px;
  position: relative;
  overflow: hidden;
}

.analytics-fill::after {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(90deg, transparent, rgba(255,255,255,0.3), transparent);
  animation: shimmer 2s infinite;
}

/* Тренды */
.trend-chart {
  display: flex;
  align-items: end;
  justify-content: space-between;
  gap: 0.5rem;
  height: 120px;
  padding: 1rem;
  background: white;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
}

.trend-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
  flex: 1;
}

.trend-label {
  font-size: 0.8rem;
  color: #6b7280;
  font-weight: 600;
}

.trend-bar {
  flex: 1;
  width: 100%;
  background: linear-gradient(180deg, #1e3c72 0%, #2a5298 100%);
  border-radius: 4px 4px 0 0;
  min-height: 10px;
  transition: all 0.3s ease;
}

.trend-bar:hover {
  filter: brightness(1.1);
}

.trend-value {
  font-size: 0.8rem;
  font-weight: 700;
  color: #1a1a2e;
}

/* Эффективность */
.efficiency-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1rem;
}

.efficiency-item {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  border: 1px solid #e5e7eb;
  text-align: center;
  transition: all 0.3s ease;
}

.efficiency-item:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0,0,0,0.1);
}

.efficiency-value {
  font-size: 1.5rem;
  font-weight: 800;
  color: #1e3c72;
  display: block;
  margin-bottom: 0.5rem;
}

.efficiency-label {
  font-size: 0.8rem;
  color: #6b7280;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}

@keyframes shimmer {
  0% { transform: translateX(-100%); }
  100% { transform: translateX(100%); }
}

/* Макет дашборда */
.dashboard-layout {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 2rem;
  margin-bottom: 3rem;
}

.main-content {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.sidebar {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.sidebar-stats {
  margin-bottom: 0;
}

/* Адаптивность */
@media (max-width: 1024px) {
  .dashboard-layout {
    grid-template-columns: 1fr;
  }
  
  .sidebar {
    order: -1;
  }
}

@media (max-width: 768px) {
  .hero-section {
    padding: 2rem 1.5rem;
  }
  
  .hero-title {
    font-size: 2rem;
  }
  
  .hero-stats {
    flex-direction: column;
    gap: 1rem;
  }
  
  .stat-card {
    padding: 1.5rem;
    flex-direction: column;
    text-align: center;
  }
  
  .stat-icon {
    align-self: center;
  }
  
  .action-card {
    flex-direction: column;
    text-align: center;
    align-items: center;
  }
  
  .event-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }
  
  .event-date {
    align-self: center;
  }
  
  .type-stat {
    flex-direction: column;
    align-items: flex-start;
    text-align: left;
  }
  
  .dashboard-layout {
    grid-template-columns: 1fr;
  }
  
  .sidebar {
    order: -1;
  }
}
</style>
