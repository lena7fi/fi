import style from '@/styles/modal.module.scss'
import type { TApp, TOs } from '@/types/Footer'
import store from '@/utils/electron-store'
import { useEffect, useState } from 'react'

const UpdateBlock = () => {
	const [osInfo, setOsInfo] = useState<TOs>()
	const [appInfo, setAppInfo] = useState<TApp>()

	useEffect(() => {
		window.ipcRenderer.invoke('get-os-info').then(res => setOsInfo(res))
		window.ipcRenderer.invoke('get-app-info').then(res => setAppInfo(res))
	}, [])

	return (
		<div className={style.modalFooterSettings}>
			<div style={{ display: 'flex', flexDirection: 'column' }}>
				<span>Stable v{appInfo?.version} (10.02.2022)</span>
				<span>
					{osInfo?.version} {osInfo?.arch} ({osInfo?.release})
				</span>
			</div>
		</div>
	)
}

export default UpdateBlock
