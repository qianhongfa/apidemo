async function apiList(entity) {
  const r = await fetch(`/api/${entity}`);
  return r.ok ? r.json() : [];
}

async function apiGet(entity, id) {
  const r = await fetch(`/api/${entity}/${id}`);
  return r.ok ? r.json() : null;
}

async function apiCreate(entity, payload) {
  const r = await fetch(`/api/${entity}`, { method: 'POST', headers: { 'Content-Type':'application/json' }, body: JSON.stringify(payload)});
  return r;
}

async function apiDelete(entity, id) {
  const r = await fetch(`/api/${entity}/${id}`, { method: 'DELETE' });
  return r;
}

function renderList(container, items, fields) {
  container.innerHTML = '';
  const table = document.createElement('table');
  table.border = '1';
  const thead = document.createElement('thead');
  const header = document.createElement('tr');
  fields.forEach(f => { const th = document.createElement('th'); th.textContent = f; header.appendChild(th); });
  header.appendChild(document.createElement('th'));
  thead.appendChild(header);
  table.appendChild(thead);
  const tbody = document.createElement('tbody');
  items.forEach(it => {
    const tr = document.createElement('tr');
    fields.forEach(f => { const td = document.createElement('td'); td.textContent = it[f] ?? ''; tr.appendChild(td); });
    const td = document.createElement('td');
    const del = document.createElement('button'); del.textContent = '删除'; del.onclick = async ()=>{ await apiDelete(entity, it.id); load(); };
    td.appendChild(del);
    tr.appendChild(td);
    tbody.appendChild(tr);
  });
  table.appendChild(tbody);
  container.appendChild(table);
}

async function setup(entity, fields) {
  window.entity = entity;
  window.fields = fields;
  const title = document.getElementById('title');
  title.textContent = `${entity}`;
  const listContainer = document.getElementById('list');
  const form = document.getElementById('addForm');
  form.innerHTML = '';
  fields.filter(f=>f!=='id').forEach(f=>{
    const input = document.createElement('input'); input.name = f; input.placeholder = f; form.appendChild(input); form.appendChild(document.createTextNode(' '));
  });
  const submit = document.createElement('button'); submit.textContent = '添加'; submit.type='button'; submit.onclick = async ()=>{
    const data = {};
    for(const el of form.querySelectorAll('input')) data[el.name] = el.value;
    await apiCreate(entity, data);
    await load();
  };
  form.appendChild(submit);

  window.load = async ()=>{
    const items = await apiList(entity);
    renderList(listContainer, items, fields);
  }
  await load();
}
