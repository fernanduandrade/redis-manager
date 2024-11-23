<script setup lang="ts">
import { ref } from 'vue'
type Connection = {
  host: string
  password: string
  port: number
  user: string
  name: string
}

const username = ref('')
const host = ref('')
const password = ref('')
const port = ref('')

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
  <Dialog v-model:visible="visible" modal header="Nova conexão" :style="{ width: '660px' }">
    <main class="flex flex-col gap-4">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div class="flex flex-col gap-1">
          <InputText v-model="username" placeholder="Username" />
        </div>

        <div class="flex flex-col gap-1">
          <InputText v-model="host" placeholder="Host" />
        </div>

        <div class="flex flex-col gap-1">
          <InputText v-model="port" placeholder="Port" />
        </div>

        <div class="flex flex-col gap-1">
          <InputText v-model="password" placeholder="Password" />
        </div>


      </div>
      <div class="flex items-center justify-center w-full">
        <Button class="w-[200px]">Conectar</Button>
      </div>

    </main>

  </Dialog>
  <main class="bg-red-900 relative min-h-screen flex">
    <div class="bg-purple-800 text-blue-100 w-64 p-4">
      <Button @click="visible = true">Adicionar nova conexão</Button>

      <section>
        <span>Gerenciar Conexões</span>
        <div class="card flex flex-col gap-5">
          <Panel :header="`${connection.host}:${connection.port}`" v-for="(connection, index) in connections"
            :key="index" toggleable :collapsed="true">
            <p class="m-0">
              pasta de cada instancia aqui
            </p>
          </Panel>
        </div>
      </section>
    </div>

    <div class="flex-1 text-2xl p-2">
      main content
    </div>
  </main>

</template>
