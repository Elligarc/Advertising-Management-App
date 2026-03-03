<!-- pages/index.vue -->
<template>
  <div class="dashboard">
    <h1 class="page-title">Панель управления</h1>
    
    <!-- Статистика -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon" aria-hidden="true"></div>
        <div class="stat-content">
          <span class="stat-label">Всего поверхностей</span>
          <span class="stat-value">45</span>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon stat-icon--green" aria-hidden="true"></div>
        <div class="stat-content">
          <span class="stat-label">Свободны</span>
          <span class="stat-value">18</span>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon stat-icon--red" aria-hidden="true"></div>
        <div class="stat-content">
          <span class="stat-label">Заняты</span>
          <span class="stat-value">21</span>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon stat-icon--yellow" aria-hidden="true"></div>
        <div class="stat-content">
          <span class="stat-label">В простое</span>
          <span class="stat-value">6</span>
        </div>
      </div>
    </div>

    <!-- Ближайшие события -->
    <div class="section">
      <h2 class="section-title">Ближайшие события</h2>
      <div class="events-list">
        <div class="event-item">
          <div class="event-date">20.03</div>
          <div class="event-info">
            <span class="event-title">Окончание аренды</span>
            <span class="event-desc">Билборд Тверская 15 - ООО Ромашка</span>
          </div>
          <span class="event-badge warning">Осталось 2 дня</span>
        </div>
        
        <div class="event-item">
          <div class="event-date">22.03</div>
          <div class="event-info">
            <span class="event-title">Начало аренды</span>
            <span class="event-desc">Ситилайт Арбат 10 - ТехноПлюс</span>
          </div>
          <span class="event-badge success">Новая аренда</span>
        </div>
        
        <div class="event-item">
          <div class="event-date">25.03</div>
          <div class="event-info">
            <span class="event-title">Окончание ремонта</span>
            <span class="event-desc">Видеоэкран Садовая 5</span>
          </div>
          <span class="event-badge repair">В ремонте</span>
        </div>
      </div>
    </div>

    <!-- Популярные поверхности -->
    <div class="section">
      <h2 class="section-title">Популярные поверхности</h2>
      <div class="popular-grid">
        <div class="popular-card">
          <h3>Билборд Тверская 15</h3>
          <p>Занятость: 85%</p>
          <div class="progress-bar">
            <div class="progress" style="width: 85%"></div>
          </div>
        </div>
        
        <div class="popular-card">
          <h3>Ситилайт Арбат 10</h3>
          <p>Занятость: 72%</p>
          <div class="progress-bar">
            <div class="progress" style="width: 72%"></div>
          </div>
        </div>
        
        <div class="popular-card">
          <h3>Билборд Ленинский 20</h3>
          <p>Занятость: 68%</p>
          <div class="progress-bar">
            <div class="progress" style="width: 68%"></div>
          </div>
        </div>
      </div>
    </div>

    <!-- График загруженности поверхностей -->
    <div class="section">
      <h2 class="section-title">Загруженность по месяцам</h2>
      <div class="load-chart">
        <div class="load-row">
          <span class="month">Янв</span>
          <div class="bar-wrap">
            <div class="bar" style="width: 60%"></div>
          </div>
          <span class="value">60%</span>
        </div>
        <div class="load-row">
          <span class="month">Фев</span>
          <div class="bar-wrap">
            <div class="bar" style="width: 72%"></div>
          </div>
          <span class="value">72%</span>
        </div>
        <div class="load-row">
          <span class="month">Мар</span>
          <div class="bar-wrap">
            <div class="bar" style="width: 85%"></div>
          </div>
          <span class="value">85%</span>
        </div>
        <div class="load-row">
          <span class="month">Апр</span>
          <div class="bar-wrap">
            <div class="bar" style="width: 78%"></div>
          </div>
          <span class="value">78%</span>
        </div>
      </div>
    </div>

    <!-- Быстрые действия -->
    <div class="quick-actions">
      <NuxtLink to="/surfaces/add" class="btn btn-primary">Добавить поверхность</NuxtLink>
      <NuxtLink to="/clients/add" class="btn btn-primary">Новый клиент</NuxtLink>
      <NuxtLink to="/downtime/add" class="btn btn-warning">Отметить простой</NuxtLink>
    </div>
  </div>
</template>

<style scoped>
.dashboard {
  animation: fadeIn 0.5s;
}

.page-title {
  font-size: 2rem;
  color: #2c3e50;
  margin-bottom: 2rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.5rem;
  margin-bottom: 3rem;
}

.stat-card {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
  transition: transform 0.3s;
}

.stat-card:hover {
  transform: translateY(-5px);
}

.stat-icon {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background: #95a5a6;
}
.stat-icon--green { background: #27ae60; }
.stat-icon--red { background: #e74c3c; }
.stat-icon--yellow { background: #f1c40f; }

.stat-content {
  display: flex;
  flex-direction: column;
}

.stat-label {
  color: #7f8c8d;
  font-size: 0.9rem;
}

.stat-value {
  font-size: 2rem;
  font-weight: bold;
  color: #2c3e50;
}

.section {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  margin-bottom: 2rem;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.section-title {
  font-size: 1.3rem;
  color: #2c3e50;
  margin-bottom: 1.5rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.events-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.event-item {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  padding: 1rem;
  background-color: #f8f9fa;
  border-radius: 8px;
  transition: background-color 0.3s;
}

.event-item:hover {
  background-color: #e9ecef;
}

.event-date {
  font-size: 1.2rem;
  font-weight: bold;
  color: #2a5298;
  min-width: 60px;
}

.event-info {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.event-title {
  font-weight: bold;
  color: #2c3e50;
}

.event-desc {
  font-size: 0.9rem;
  color: #7f8c8d;
}

.event-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 600;
}

.event-badge.warning {
  background-color: #fff3cd;
  color: #856404;
}

.event-badge.success {
  background-color: #d4edda;
  color: #155724;
}

.event-badge.repair {
  background-color: #f8d7da;
  color: #721c24;
}

.popular-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
}

.popular-card {
  background-color: #f8f9fa;
  border-radius: 8px;
  padding: 1rem;
}

.popular-card h3 {
  font-size: 1rem;
  margin-bottom: 0.5rem;
  color: #2c3e50;
}

.popular-card p {
  font-size: 0.9rem;
  color: #7f8c8d;
  margin-bottom: 0.5rem;
}

.progress-bar {
  height: 8px;
  background-color: #e0e0e0;
  border-radius: 4px;
  overflow: hidden;
}

.progress {
  height: 100%;
  background: linear-gradient(135deg, #1e3c72, #2a5298);
  border-radius: 4px;
}

.load-chart {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.load-row {
  display: grid;
  grid-template-columns: 40px 1fr 48px;
  align-items: center;
  gap: 0.75rem;
  font-size: 0.9rem;
  color: #4b5563;
}

.load-row .month {
  font-weight: 600;
}

.bar-wrap {
  background: #e5e7eb;
  border-radius: 999px;
  overflow: hidden;
  height: 10px;
}

.bar {
  height: 100%;
  background: linear-gradient(135deg, #1e3c72, #2a5298);
  border-radius: 999px;
}

.load-row .value {
  text-align: right;
  font-variant-numeric: tabular-nums;
}

.quick-actions {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
  margin-top: 2rem;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>