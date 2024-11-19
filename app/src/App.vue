<script setup lang="ts">
import { ref } from 'vue'
type Connection = {
  host: string
  password: string
  port: number
  user: string
  name: string
}

const visible = ref(false)

const connections = ref<Array<Connection>>([
  { host: 'localhost', port: 6339, password: '', user: 'localhost', name: 'teste_1' },
  { host: 'localhost', port: 6339, password: '', user: 'localhost', name: 'teste_2' },
  { host: 'localhost', port: 6339, password: '', user: 'localhost', name: 'teste_3' },
  { host: 'localhost', port: 6339, password: '', user: 'localhost', name: 'teste_4' },
  { host: 'localhost', port: 6339, password: '', user: 'localhost', name: 'teste_5' },
  { host: 'localhost', port: 6339, password: '', user: 'localhost', name: 'teste_6' }
])

</script>

<template>
  <Dialog v-model:visible="visible" modal header="Nova conexão" :style="{ width: '25rem' }">
    weeee
  </Dialog>
  <main class="bg-red-900 relative min-h-screen flex">
    <div class="bg-purple-800 text-blue-100 w-64 p-4">
      <Button @click="visible = true">Adicionar nova conexão</Button>

      <section>
        <span>Conexões</span>
        <div v-for="(connection, index) in connections" :key="index">
          <Button>Conectar</Button>
        </div>
      </section>
    </div>

    <div class="flex-1 text-2xl p-2">
      main content
    </div>
  </main>
    
</template>
