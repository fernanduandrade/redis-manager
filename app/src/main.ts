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
import Divider from 'primevue/divider'
import AutoComplete from 'primevue/autocomplete'
import Select from 'primevue/select'
import 'primeicons/primeicons.css'
import { createPinia } from 'pinia'
import Splitter from 'primevue/splitter'
import SplitterPanel from 'primevue/splitterpanel'
import ContextMenu from 'primevue/contextmenu'

const pinia = createPinia()
const app = createApp(App)

app.use(pinia)
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
    .component('Divider', Divider)
    .component('AutoComplete', AutoComplete)
    .component('Select', Select)
    .component('Splitter', Splitter)
    .component('SplitterPanel', SplitterPanel)
    .component('ContextMenu', ContextMenu)
app.mount('#app')
