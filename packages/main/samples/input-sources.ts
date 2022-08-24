import { 
	BrowserWindow, 
	// desktopCapturer, 
	ipcMain,
	Menu } from 'electron'

const { ConnectionBuilder } = require('electron-cgi')

const connection = new ConnectionBuilder()
  .connectTo('dotnet', 'run', '--project', './core/Core.csproj')
  .build()

connection.onDisconnect = () => {
  console.log('lost');
}

ipcMain.on('show-input-sources', (event, sources: string) => {
	const template = JSON.parse(sources).map((source:any) => {
		return {
			label: source.name,
			click: () => {
				connection.send('focus-source', JSON.stringify(source), (err, res) => {
					console.log(res)
					event.sender.send('input-source-selected', source)
				})
			},
		}
	})

	Menu.buildFromTemplate(template).popup(
		BrowserWindow.fromWebContents(event.sender) as Electron.PopupOptions
	)
})


ipcMain.handle('get-input-sources', async (event, types: []) => {

	const sources = await connection.send('fetch-sources', 'Fi')
	// const sources = await desktopCapturer.getSources({types})
	console.log(sources)
	return sources
})

ipcMain.on('reset-source', event => {
	event.sender.send('input-source-selected')
})
