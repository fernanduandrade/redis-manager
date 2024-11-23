import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import PrimeVue from 'primevue/config'
import Aura from '@primevue/themes/aura'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import InputGroup from 'primevue/inputgroup'
import InputGroupAddon from 'primevue/inputgroupaddon'
import { InputText } from 'primevue'
import Panel from 'primevue/panel'

const app = createApp(App)

app.use(PrimeVue, {
    theme: {
        preset: Aura
    }
})
app
    .component('Button', Button)
    .component('Dialog', Dialog)
    .component('InputGroup', InputGroup)
    .component('InputGroupAddon', InputGroupAddon)
    .component('InputText', InputText)
    .component('Panel', Panel);
app.mount('#app')
