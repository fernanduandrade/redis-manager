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

const teste = ref<any>([])
async function loadKeyspaces() {
  const response = await fetch('https://localhost:7261/api/redis/keyspaces?id=04fb6731-59d0-4ef3-be5c-f8ea439c3966&host=localhost&port=6379')
  const result = await response.json()

  teste.value = result
}

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
  <main class="relative min-h-screen flex">
    <div class="bg-[#F9FAFE] w-64 p-4 flex flex-col items-center gap-5 border-r-2 border-red-gray">
      <Button @click="visible = true">Adicionar nova conexão</Button>
      <section>
        <div class="card flex flex-col gap-2">
          <Panel :header="`${connection.host}:${connection.port}`" v-for="(connection, index) in connections"
            :key="index" toggleable :collapsed="true" @toggle="loadKeyspaces">
            <p class="m-0" v-for="(aa, index) in teste" :key="index">
              <i class="pi pi-folder"></i> {{ aa.name }} ({{aa.count}}) 
            </p>
          </Panel>
        </div>
      </section>
    </div>

    <div class="flex-1 p-4 text-2xl bg-[#F9FAFE]">
      main content
    </div>
  </main>

</template>
