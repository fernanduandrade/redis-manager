<script lang="ts" setup>
import { reactive, ref } from 'vue';
import { Key } from '../../domain'
import redis from '../../../api/redis'
const emit = defineEmits(['closeForm'])

const props = defineProps({
    visible: {
        type: Boolean
    },
    connectionId: {
        type: String,
        required: true
    }
})

const keyErrorMessages = ref('')

const newKey = reactive<Key>({
    key: '',
    value: '',
})

const loadKey = ref(false)

async function addKey() {
    loadKey.value = true
    

    const result = await redis.createKeyValue(props.connectionId, newKey.key, newKey.value)
    if (result === null) {
        keyErrorMessages.value = "Não foi possível criar a chave"
        loadKey.value = !loadKey.value
        return
    }

    hideForm()
}

function hideForm() {
    emit('closeForm', false)
    newKey.value = ''
    newKey.key = ''
    loadKey.value = false
}
</script>

<template>
    <Dialog v-model:visible="props.visible" modal header="Nova chave" :style="{ width: '660px' }"
        @update:visible="hideForm">
        <main class="flex flex-col gap-4">
            <div class="grid grid-cols-1 md:grid-cols-1 gap-4">

                <div class="flex flex-col gap-1">
                    <label for="key">Chave *</label>
                    <InputText id="key" v-model="newKey.key" placeholder="Chave" />
                </div>

                <div class="flex flex-col gap-1">
                    <label for="value">Valor</label>
                    <textarea class="hover:border-solid hover:border-2 hover:border-cyan-100 h-[150px]" id="value" v-model="newKey.value" placeholder="Valor" />
                </div>
            </div>
            <div class="flex items-center justify-end w-full">
                <Button :loading="loadKey" class="w-[200px]" label="Conectar" @click="addKey" />
            </div>
            <span class="text-red-600" v-if="keyErrorMessages">
                {{ keyErrorMessages }}
            </span>

        </main>

    </Dialog>
</template>