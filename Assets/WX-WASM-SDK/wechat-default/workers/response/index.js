// 消息类型
const messageType = {
  config: 0, // 检查是否支持worker写文件
  writeFile: 1, // 写文件
}

const fs = worker.getFileSystemManager ? worker.getFileSystemManager() : null;
const createSharedArrayBuffer = worker.createSharedArrayBuffer;

worker.onMessage((res) => {
  const {type, payload} = res;
  if (type === messageType.writeFile) {
    const {filePath, data, isSharedBuffer} = payload
    let content = data
    if (isSharedBuffer) {
      content = data.buffer
    }
    fs.writeFile({
      filePath,
      data: content,
      success: () => {
        worker.postMessage({
          type: messageType.writeFile,
          payload: {
            isok: true,
            filePath,
          }
        })
      },
      fail: err => {
        worker.postMessage({
          type: messageType.writeFile,
          payload: {
            isok: false,
            filePath,
            err
          }
        })
      }
    })
  }
  if (type === messageType.config) {
    const {isAndroid} = payload

    worker.postMessage({
      type: messageType.config,
      payload: {
        supportWorkerFs: isAndroid && !!fs,
        supportSharedBuffer: isAndroid && !!createSharedArrayBuffer,
      }
    })
  }
})

