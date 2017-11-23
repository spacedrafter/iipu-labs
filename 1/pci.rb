require_relative 'device.rb'

PCI_DEVICES_FILE = '/proc/bus/pci/devices'.freeze
PCI_IDS_FILE = '/usr/share/hwdata/pci.ids'.freeze

pci_id = Array.new
devices_list = Array.new
device_name = nil

File.open(PCI_DEVICES_FILE) do |file|
 pci_id = file.map {|line| line.split("\t")[1]}.map{|find_id| find_id.scan(/..../)}
end

pci_id.each do |device|
  devices_list.push(Device.new(device[0],device[1]))
end

File.open(PCI_IDS_FILE) do |file|

  file.each do |line|
    next if line =~ /^\t{2}.*/ or line[0] == '#'
    if line[0] != "\t"
      vendor_id = line[0..3]
      vendor_name = line[4..-1]
    else
      device_id = line[1..4]
      device_name = line[5..-1]
    end

    devices_list.each do |device|
      device.vendor_name = vendor_name if device.vendor_id == vendor_id
      device.device_name = device_name if device.device_id == device_id
    end
  end

end

print " "
devices_list.each.with_index do |device, index|
  print "#{index + 1}."
  device.output
end
