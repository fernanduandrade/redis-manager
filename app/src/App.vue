<script setup lang="ts">
import redis from './api/redis'
import { Connection, RedisKey } from './common/domain';
import { ref } from 'vue'
import ConnectionForm from './common/components/ConnectionForm/index.vue'
import KeyFolder from './common/components/KeyFolder/index.vue'

const showConnectionFormModal = ref(false)
function closeFormEvent(evt: boolean) {
  showConnectionFormModal.value = evt
}
const connections = ref<Array<Connection>>([
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_1', open: false },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: '', open: false },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_3', open: false },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: '', open: false },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: '', open: false },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_6', open: false }
])

const teste: Array<RedisKey> = [
  { expanded: false, id: '1', name: 'alves', type: 'keyspace', children: [], count: 0 },
  { expanded: false, id: '2', name: 'ferreira', type: 'keyspace', children: [], count: 0 },
  { expanded: false, id: '3', name: 'joao', type: 'keyspace', children: [], count: 0 },
  { expanded: false, id: '4', name: 'truta', type: 'keyspace', children: [], count: 0 },
  { expanded: false, id: '5', name: 'sp', type: 'keyspace', children: [], count: 0 },
  {
    expanded: false, id: '6', name: 'goiais', type: 'keyspace', children: [
      { expanded: false, id: '5', name: 'nando', type: 'key', children: [], count: 0 },
    ], count: 1
  },
  {
    expanded: false, id: '7', name: 'algo - teste', type: 'keyspace', children: [
      { expanded: false, id: '1', name: 'algo filho 1', type: 'key', children: [], count: 0 },
      { expanded: false, id: '2', name: 'algo filho 2', type: 'key', children: [], count: 0 },
      { expanded: false, id: '3', name: 'algo filho 3', type: 'key', children: [], count: 0 },
      { expanded: false, id: '4', name: 'algo filho 4', type: 'key', children: [], count: 0 },
      { expanded: false, id: '5', name: 'algo filho 5', type: 'key', children: [], count: 0 },
      { expanded: false, id: '6', name: 'algo filho 6', type: 'key', children: [], count: 0 },
    ], count: 6
  },
  { expanded: false, id: '8', name: 'andrade1', type: 'keyspace', children: [], count: 0 },
  { expanded: false, id: '12', name: 'aabbcc', type: 'keyspace', children: [], count: 0 },
  { expanded: false, id: '10', name: 'teste', type: 'keyspace', children: [], count: 0 },
]

const textValueExample = ref('')
function connectionName(connection: Connection) {
  if (connection.name)
    return connection.name

  return `${connection.host}@${connection.port}`
}

function openConnection(connection: Connection) {
  connection.open = !connection.open
}
</script>

<template>
  <ConnectionForm :visible="showConnectionFormModal" @close-form="closeFormEvent" />
  <main class="relative min-h-screen flex">
    <div class="bg-[#FFFFFF] w-[500px] p-7 flex flex-col items-center gap-5 border-r-2 border-red-gray">
      <Button @click="showConnectionFormModal = true">Adicionar nova conexão</Button>
      <section>
        <div class="card flex flex-col gap-2">
          <div class="bg-white flex flex-col p-4" v-for="(connection, index) in connections" :key="index">
            <div class="flex p-4 justify-between cursor-pointer hover:bg-slate-300 transition-all" @click="openConnection(connection)">
              <span class="font-semibold">{{ connectionName(connection) }}</span>
              <div class="flex gap-3 items-center">
                <div class="connection__action flex items-center justify-center rounded-md hover:bg-green-200 z-10 cursor-pointer"><i class="pi pi-home" /></div>
                <div class="connection__action flex items-center justify-center rounded-md hover:bg-green-200 cursor-pointer"><i class="pi pi-bars" /></div>
                <div :class="[connection.open ? 'rotate' : '']" ><i class="pi pi-chevron-down" /></div>
              </div>

            </div>

            <div v-if="connection.open" class="p-5 flex flex-col gap-4">
              <AutoComplete v-model="textValueExample" placeholder="Enter para pesquisar" :suggestions="teste" />
              <KeyFolder :items="teste" />
            </div>
          </div>
        </div>
      </section>
    </div>

    <div class="flex-1 flex justify-center items-center p-4 text-2xl bg-[#F9FAFE]">
      <p class="text-[100px] font-semibold">se liga aqui mané</p>
      <br />
      <p class="text-[100px] font-semibold">{{ textValueExample }}</p>
    </div>
  </main>

</template>

<style scoped>
.connection__action {
  width: 30px;
  height: 30px;
}

.rotate {
  animation: rotation .4s forwards;
}

@keyframes rotation {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(180deg);
  }
}
</style>