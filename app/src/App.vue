<script setup lang="ts">
import redis from './api/redis'
import { Connection, RedisKey } from './common/domain';
import { ref } from 'vue'
import ConnectionForm from './common/components/ConnectionForm/index.vue'
import Folder from './common/components/KeyFolder/index.vue'

const showConnectionFormModal = ref(false)
function closeFormEvent(evt: boolean) {
  showConnectionFormModal.value = evt
}
const connections = ref<Array<Connection>>([
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_1' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_2' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_3' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_4' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_5' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_6' }
])

const teste: Array<RedisKey> = [
    {expanded: false, id: '1', name: 'alves', type: 'keyspace', children: [], count: 0},
    {expanded: false, id: '2', name: 'ferreira', type: 'keyspace', children: [], count: 0 },
    {expanded: false, id: '3', name: 'joao', type: 'keyspace', children: [], count: 0},
    {expanded: false, id: '4', name: 'truta', type: 'keyspace', children: [], count: 0},
    {expanded: false, id: '5', name: 'sp', type: 'keyspace', children: [], count: 0},
    {expanded: false, id: '6', name: 'goiais', type: 'keyspace', children: [
    {expanded: false, id: '5', name: 'nando', type: 'key', children: [], count: 0},
    ], count: 1},
    {expanded: false, id: '7', name: 'algo - teste', type: 'keyspace', children: [
        {expanded: false, id: '1', name: 'algo filho 1', type: 'key', children: [], count: 0},
        {expanded: false, id: '2', name: 'algo filho 2', type: 'key', children: [], count: 0},
        {expanded: false, id: '3', name: 'algo filho 3', type: 'key', children: [], count: 0},
        {expanded: false, id: '4', name: 'algo filho 4', type: 'key', children: [], count: 0},
        {expanded: false, id: '5', name: 'algo filho 5', type: 'key', children: [], count: 0},
        {expanded: false, id: '6', name: 'algo filho 6', type: 'key', children: [], count: 0},
    ], count: 6},
    {expanded: false, id: '8', name: 'andrade1', type: 'keyspace', children: [], count: 0},
    {expanded: false, id: '12', name: 'aabbcc', type: 'keyspace', children: [], count: 0},
    {expanded: false, id: '10', name: 'teste', type: 'keyspace', children: [], count: 0},
]

const textValueExample = ref('')

// TODO componentizar para resolver problema de keyspaces que tem filhos
// TODO usar data tree data structure
</script>

<template>
  <ConnectionForm :visible="showConnectionFormModal" @close-form="closeFormEvent" />
  <main class="relative min-h-screen flex">
    <div class="bg-[#F9FAFE] w-[300px] p-4 flex flex-col items-center gap-5 border-r-2 border-red-gray">
      <Button @click="showConnectionFormModal = true">Adicionar nova conexão</Button>
      <section>
        <div class="card flex flex-col gap-2">
          <Panel :header="`${connection.host}:${connection.port}`" v-for="(connection, index) in connections"
            :key="index" toggleable :collapsed="true">
              <Folder :items="teste" />
          </Panel>
        </div>
      </section>
    </div>

    <div class="flex-1 flex justify-center items-center p-4 text-2xl bg-[#F9FAFE]">
      <p class="text-[100px] font-semibold">se liga aqui mané</p>
      <br/>
      <p class="text-[100px] font-semibold">{{ textValueExample }}</p>
    </div>
  </main>

</template>
