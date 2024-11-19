import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
const app = createApp(App)

app.use(PrimeVue, {
    theme: {
        preset: Aura
    }
})
app.component('Button', Button).component('Dialog', Dialog);
app.mount('#app')
