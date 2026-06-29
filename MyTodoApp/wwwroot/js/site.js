const addTaskButton = document.querySelector(".add-task");

addTaskButton.addEventListener('click', async () => {
  const inputElement = document.getElementById('task-name');
  const taskName = inputElement.value.trim();

  if (taskName === "") {
    alert("Nejdřív napiš název úkolu!");
    return;
  }

  const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
  const token = tokenElement ? tokenElement.value : "";

  const response = await fetch('?handler=CreateTask', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
      'RequestVerificationToken': token
    },
    body: new URLSearchParams({ 'taskName': taskName })
  });

  if (response.ok) {
    const data = await response.json();

    const taskHtml = `
        <div class="task">
          <h2>${data.name}</h2>
          <input type="checkbox" name="task" value="Task" />
        </div>
      `;

    document.getElementById('task-container').insertAdjacentHTML('beforeend', taskHtml);
    inputElement.value = "";
  } else {
    const errorText = await response.text();
    alert(`Něco se nepovedlo na serveru!\nKód: ${response.status}\nDůvod: ${errorText}`);
  }
});