GET_HDD_INFO_COMMAND = `sudo hdparm -I /dev/sda`.freeze
HDD_ARRAY = [/Model Number/ ,/Firmware Revision/ , /Serial Number/,
              /^Transport/, /^PIO/, /^DMA/]
GET_MEMORY_INFO_COMMAND = `df -m`.freeze


properties = GET_HDD_INFO_COMMAND
properties = properties.split("\n").map{ |item| item.strip}

memory_info = GET_MEMORY_INFO_COMMAND
memory_info = memory_info.split("\n").drop(1)
memory_array = Array.new(3, 0)
memory_info.each do |line|
  memory_array.map!.with_index{ |elem, index| elem += line.split(/\s+/)[index+1].to_i}
end

HDD_ARRAY.each do |item|
  properties.each { |prop| puts prop if prop =~ item }
end
puts "Memory Info:\nTotal:#{memory_array[0]} MB\nUsed:#{memory_array[1]} MB\nFree:#{memory_array[2]} MB"
