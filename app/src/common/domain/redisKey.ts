export type RedisKey = {
    id: string
    type: KeyType
    name: string
    expanded: boolean
    children?: Array<RedisKey>
    count: number
    parent: string
}

export type KeyType = 'keySpace' | 'key'